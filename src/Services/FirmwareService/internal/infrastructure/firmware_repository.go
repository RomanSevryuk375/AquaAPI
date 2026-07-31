package infrastructure

import (
	"context"
	"database/sql"
	"errors"
	"fmt"

	"github.com/google/uuid"
	"github.com/jmoiron/sqlx"
	"github.com/romansevryuk375/aquasmart-firmware/internal/domain/entities"
	"github.com/romansevryuk375/aquasmart-firmware/internal/domain/vo"
)

type firmwareDAO struct {
	ID                uuid.UUID `db:"id"`
	HardwareProfileID uuid.UUID `db:"hardware_profile_id"`
	Version           string    `db:"version"`
	FileHash          string    `db:"file_hash"`
	SizeBytes         int       `db:"size_bytes"`
	StorageKey        string    `db:"storage_key"`
	Status            int       `db:"status"`
	RevokeReason      *string   `db:"revoke_reason"`
}

type PostgresFirmwareRepository struct {
	db *sqlx.DB
}

func NewPostgresFirmwareRepository(db *sqlx.DB) *PostgresFirmwareRepository {
	return &PostgresFirmwareRepository{db: db}
}

func (r *PostgresFirmwareRepository) GetById(ctx context.Context, id uuid.UUID) (*entities.Firmware, error) {
	query := `
		SELECT id, hardware_profile_id, version, file_hash, size_bytes, storage_key, status, revoke_reason
		FROM firmwares
		WHERE id = $1
	`
	var dao firmwareDAO
	err := r.db.GetContext(ctx, &dao, query, id)
	if errors.Is(err, sql.ErrNoRows) {
		return nil, nil
	}
	if err != nil {
		return nil, fmt.Errorf("failed to query firmware by id: %w", err)
	}

	version, err := vo.ParseVersion(dao.Version)
	if err != nil {
		return nil, fmt.Errorf("failed to parse version from db: %w", err)
	}

	revokeReason := ""
	if dao.RevokeReason != nil {
		revokeReason = *dao.RevokeReason
	}

	fw := entities.LoadFirmwareFromDB(
		dao.ID,
		dao.HardwareProfileID,
		*version,
		dao.FileHash,
		dao.SizeBytes,
		dao.StorageKey,
		entities.FirmwareStatus(dao.Status),
		revokeReason,
	)

	return fw, nil
}

func (r *PostgresFirmwareRepository) Save(ctx context.Context, fw *entities.Firmware) error {
	v := fw.Version()
	dao := firmwareDAO{
		ID:                fw.ID(),
		HardwareProfileID: fw.HardwareProfileId(),
		Version:           v.Value(),
		FileHash:          fw.FileHash(),
		SizeBytes:         fw.SizeBytes(),
		StorageKey:        fw.StorageKey(),
		Status:            int(fw.Status()),
	}

	revokeReason := fw.RevokeReason()
	if revokeReason != "" {
		dao.RevokeReason = &revokeReason
	}

	query := `
		INSERT INTO firmwares (id, hardware_profile_id, version, file_hash, size_bytes, storage_key, status, revoke_reason)
		VALUES (:id, :hardware_profile_id, :version, :file_hash, :size_bytes, :storage_key, :status, :revoke_reason)
		ON CONFLICT (id) DO UPDATE SET
			status = EXCLUDED.status,
			storage_key = EXCLUDED.storage_key,
			revoke_reason = EXCLUDED.revoke_reason
	`
	if _, err := r.db.NamedExecContext(ctx, query, dao); err != nil {
		return fmt.Errorf("failed to upsert firmware: %w", err)
	}

	return nil
}

func (r *PostgresFirmwareRepository) Delete(ctx context.Context, fw *entities.Firmware) error {
	query := `
		DELETE FROM firmwares 
		WHERE id = $1
	`
	if _, err := r.db.ExecContext(ctx, query, fw.ID()); err != nil {
		return fmt.Errorf("failed to delete firmware: %w", err)
	}

	return nil
}
