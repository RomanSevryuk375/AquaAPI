package presentation

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"

	"github.com/google/uuid"
	"github.com/romansevryuk375/aquasmart-firmware/internal/application"
	"github.com/romansevryuk375/aquasmart-firmware/internal/domain/entities"
)

type OTAResponse struct {
	HasUpdate   bool   `json:"has_update"`
	Version     string `json:"version,omitempty"`
	DownloadURL string `json:"download_url,omitempty"`
	FileHash    string `json:"file_hash,omitempty"`
}

type UpdateTargetStatusRequest struct {
	CampaignId   uuid.UUID `json:"campaign_id"`
	ControllerId uuid.UUID `json:"controller_id"`
	Status       int       `json:"status"`
	ErrorMessage string    `json:"error_message"`
}

type OTAHandler struct {
	otaService  *application.OtaDistributionService
	campService *application.CampaignService
}

func NewOTAHandler(otaService *application.OtaDistributionService, campService *application.CampaignService) *OTAHandler {
	return &OTAHandler{
		otaService:  otaService,
		campService: campService,
	}
}

func (h *OTAHandler) CheckUpdate(w http.ResponseWriter, r *http.Request) {
	controllerIdStr := r.PathValue("id")
	if controllerIdStr == "" {
		http.Error(w, "missing controller_id parameter", http.StatusBadRequest)
		return
	}

	controllerId, err := uuid.Parse(controllerIdStr)
	if err != nil {
		http.Error(w, "invalid controller_id format", http.StatusBadRequest)
		return
	}

	meta, err := h.otaService.CheckForUpdate(r.Context(), controllerId)
	if err != nil {
		log.Printf("[ERROR] Failed to check OTA for controller %s: %v", controllerId, err)
		http.Error(w, "internal server error", http.StatusInternalServerError)
		return
	}

	response := OTAResponse{
		HasUpdate:   meta.HasUpdate,
		Version:     meta.Version,
		DownloadURL: meta.DownloadURL,
		FileHash:    meta.FileHash,
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusOK)

	if err := json.NewEncoder(w).Encode(response); err != nil {
		log.Printf("[ERROR] Failed to encode OTA response: %v", err)
	}
}

func (h *OTAHandler) HandleUpdateTargetStatus(w http.ResponseWriter, r *http.Request) {
	r.Body = http.MaxBytesReader(w, r.Body, 1*1024*1024)

	var req UpdateTargetStatusRequest
	decoder := json.NewDecoder(r.Body)
	decoder.DisallowUnknownFields()
	if err := decoder.Decode(&req); err != nil {
		http.Error(w, fmt.Sprintf("invalid request body: %v", err), http.StatusBadRequest)
		return
	}

	if err := h.campService.HandleTargetStatusUpdate(
		r.Context(),
		req.CampaignId,
		req.ControllerId,
		entities.TargetStatus(req.Status),
		req.ErrorMessage); err != nil {
		log.Printf("[ERROR] Failed to target update %s: %v", req.CampaignId, err)
		http.Error(w, "internal server error", http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusNoContent)
}
