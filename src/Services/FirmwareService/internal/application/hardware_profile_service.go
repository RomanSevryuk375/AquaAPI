package application

import (
	"context"
	"errors"
	"fmt"

	"github.com/google/uuid"
	"github.com/romansevryuk375/aquasmart-firmware/internal/domain/entities"
)

type HardwareProfileService struct {
	hardwareProfileRepo HardwareProfileRepository
}

func NewHardwareProfileService(hardwareProfileRepo HardwareProfileRepository) *HardwareProfileService {
	return &HardwareProfileService{
		hardwareProfileRepo: hardwareProfileRepo,
	}
}

func (s *HardwareProfileService) CreateProfile(ctx context.Context, name string, boardRevision string) (uuid.UUID, error) {
	hp, err := entities.NewHardwareProfile(uuid.New(), name, boardRevision)
	if err != nil {
		return uuid.Nil, fmt.Errorf("failed to create new profile: %w", err)
	}

	if err = s.hardwareProfileRepo.Save(ctx, hp); err != nil {
		return uuid.Nil, fmt.Errorf("filed to save hardware profile: %w", err)
	}
	return hp.ID(), nil
}

func (s *HardwareProfileService) DeprecateProfile(ctx context.Context, id uuid.UUID) error {
	hp, err := s.hardwareProfileRepo.GetById(ctx, id)
	if err != nil {
		return fmt.Errorf("failed to get harware profile: %w", err)
	}
	if hp == nil {
		return errors.New("hardware profile not found")
	}

	hp.Deprecate()
	if err = s.hardwareProfileRepo.Save(ctx, hp); err != nil {
		return fmt.Errorf("filed to save hardware profile: %w", err)
	}
	return nil
}

func (s *HardwareProfileService) RenameProfile(ctx context.Context, id uuid.UUID, name string) error {
	hp, err := s.hardwareProfileRepo.GetById(ctx, id)
	if err != nil {
		return fmt.Errorf("failed to get harware profile: %w", err)
	}
	if hp == nil {
		return errors.New("harware profile not found")
	}

	if err = hp.UpdateName(name); err != nil {
		return fmt.Errorf("failed to update profile name: %w", err)
	}

	if err = s.hardwareProfileRepo.Save(ctx, hp); err != nil {
		return fmt.Errorf("filed to save hardware profile: %w", err)
	}
	return nil
}
