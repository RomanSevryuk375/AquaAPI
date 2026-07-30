package application

import (
	"context"
	"fmt"
	"io"

	"github.com/google/uuid"
	"github.com/romansevryuk375/aquasmart-firmware/internal/domain/entities"
	"github.com/romansevryuk375/aquasmart-firmware/internal/domain/vo"
)

type FirmwareService struct {
	firmwareRepository FirmwareRepository
	blobStorage        BlobStorage
}

func NewFirmwareService(firmwareRepository FirmwareRepository, blobStorage BlobStorage) *FirmwareService {
	return &FirmwareService{
		firmwareRepository: firmwareRepository,
		blobStorage:        blobStorage,
	}
}

func (s *FirmwareService) UploadNewRelease(ctx context.Context, hwId uuid.UUID, version string, fileHash string, sizeBytes int, fileStream io.Reader) (uuid.UUID, error) {
	ver, err := vo.ParseVersion(version)
	if err != nil {
		return uuid.Nil, fmt.Errorf("failed to parse version: %w", err)
	}

	fw, err := entities.NewFirmwareRelease(uuid.New(), hwId, *ver, fileHash, sizeBytes)
	if err != nil {
		return uuid.Nil, fmt.Errorf("failed to create new release: %w", err)
	}

	sk, err := s.blobStorage.UploadFile(ctx, fileStream, sizeBytes)
	if err != nil {
		return uuid.Nil, fmt.Errorf("failed to upload file: %w", err)
	}
	if err = fw.SetStorageKey(sk); err != nil {
		return uuid.Nil, fmt.Errorf("failed to set storage key: %w", err)
	}

	if err = s.firmwareRepository.Save(ctx, fw); err != nil {
		return uuid.Nil, fmt.Errorf("failed to save firmware release: %w", err)
	}
	return fw.ID(), nil
}
