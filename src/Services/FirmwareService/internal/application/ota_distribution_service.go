package application

import (
	"context"
	"errors"
	"fmt"

	"github.com/google/uuid"
	"github.com/romansevryuk375/aquasmart-firmware/internal/domain/entities"
)

type UpdateMetadata struct {
	HasUpdate   bool
	Version     string
	DownloadURL string
	FileHash    string
}

type OtaDistributionService struct {
	campaignRepository CampaignRepository
	firmwareRepository FirmwareRepository
	blobStorage        BlobStorage
}

func NewOtaDistributionService(campaignRepository CampaignRepository, firmwareRepository FirmwareRepository, blobStorage BlobStorage) *OtaDistributionService {
	return &OtaDistributionService{
		campaignRepository: campaignRepository,
		firmwareRepository: firmwareRepository,
		blobStorage:        blobStorage,
	}
}

func (s *OtaDistributionService) CheckForUpdate(ctx context.Context, controllerId uuid.UUID) (*UpdateMetadata, error) {
	camp, err := s.campaignRepository.GetActiveCampaignForController(ctx, controllerId)
	if err != nil {
		return nil, fmt.Errorf("failed to get campaign: %w", err)
	}
	if camp == nil {
		return &UpdateMetadata{
			HasUpdate: false,
		}, nil
	}

	val, exists := camp.FindTargets(controllerId)
	if !exists {
		return nil, fmt.Errorf("target controller %v not found", controllerId)
	}
	if val.Status() == entities.TargetSuccess {
		return &UpdateMetadata{
			HasUpdate: false,
		}, nil
	}

	fw, err := s.firmwareRepository.GetByID(ctx, camp.FirmwareId())
	if err != nil {
		return nil, fmt.Errorf("failed to get firmware: %w", err)
	}
	if fw == nil {
		return nil, errors.New("firmware not found")
	}

	downloadUrl, err := s.blobStorage.GeneratePreSignedURL(ctx, fw.StorageKey())
	if err != nil {
		return nil, fmt.Errorf("failed to get download url: %w", err)
	}
	version := fw.Version()
	return &UpdateMetadata{
		HasUpdate:   true,
		Version:     version.Value(),
		DownloadURL: downloadUrl,
		FileHash:    fw.FileHash(),
	}, nil
}
