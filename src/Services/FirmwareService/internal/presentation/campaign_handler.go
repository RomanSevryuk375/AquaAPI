package presentation

import (
	"encoding/json"
	"errors"
	"fmt"
	"log"
	"net/http"

	"github.com/google/uuid"
	"github.com/romansevryuk375/aquasmart-firmware/internal/application"
)

type CreateCampaignRequest struct {
	FirmwareId uuid.UUID `json:"firmware_id"`
	Name       string    `json:"name"`
}

type AddTargetsRequest struct {
	ControllerIds []uuid.UUID `json:"controller_ids"`
}

type CancelCampaignRequest struct {
	Reason string `json:"reason"`
}

type CampaignHandler struct {
	campaignService *application.CampaignService
}

func NewCampaignHandler(campaignService *application.CampaignService) *CampaignHandler {
	return &CampaignHandler{
		campaignService: campaignService,
	}
}

func (h *CampaignHandler) HandleCreateCampaign(w http.ResponseWriter, r *http.Request) {
	r.Body = http.MaxBytesReader(w, r.Body, 1*1024*1024)

	var req CreateCampaignRequest
	decoder := json.NewDecoder(r.Body)
	decoder.DisallowUnknownFields()
	if err := decoder.Decode(&req); err != nil {
		http.Error(w, fmt.Sprintf("invalid request body: %v", err), http.StatusBadRequest)
		return
	}

	if req.Name == "" {
		http.Error(w, "name is required", http.StatusBadRequest)
		return
	}
	if req.FirmwareId == uuid.Nil {
		http.Error(w, "firmware_id is required", http.StatusBadRequest)
		return
	}

	camp, err := h.campaignService.CreateCampaign(r.Context(), req.FirmwareId, req.Name)
	if err != nil {
		log.Printf("[ERROR] Failed to create campaign: %v", err)
		http.Error(w, "internal server error", http.StatusInternalServerError)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	fmt.Fprintf(w, `{"id":"%s"}`, camp.ID())
}

func (h *CampaignHandler) HandleAddTargets(w http.ResponseWriter, r *http.Request) {
	campaignId, err := parseUUIDFromPath(r, "id")
	if err != nil {
		http.Error(w, "invalid campaign id format", http.StatusBadRequest)
		return
	}

	r.Body = http.MaxBytesReader(w, r.Body, 5*1024*1024)

	var req AddTargetsRequest
	decoder := json.NewDecoder(r.Body)
	decoder.DisallowUnknownFields()
	if err := decoder.Decode(&req); err != nil {
		http.Error(w, fmt.Sprintf("invalid request body: %v", err), http.StatusBadRequest)
		return
	}

	if len(req.ControllerIds) == 0 {
		http.Error(w, "controller_ids cannot be empty", http.StatusBadRequest)
		return
	}

	if err := h.campaignService.AddTargetsToCampaign(r.Context(), campaignId, req.ControllerIds); err != nil {
		log.Printf("[ERROR] Failed to add targets to campaign %s: %v", campaignId, err)
		http.Error(w, "internal server error", http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusNoContent)
}

func (h *CampaignHandler) HandleStartCampaign(w http.ResponseWriter, r *http.Request) {
	campaignId, err := parseUUIDFromPath(r, "id")
	if err != nil {
		http.Error(w, "invalid campaign id format", http.StatusBadRequest)
		return
	}

	if err := h.campaignService.StartCampaign(r.Context(), campaignId); err != nil {
		log.Printf("[ERROR] Failed to start campaign %s: %v", campaignId, err)
		http.Error(w, "internal server error", http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusNoContent)
}

func (h *CampaignHandler) HandlePauseCampaign(w http.ResponseWriter, r *http.Request) {
	campaignId, err := parseUUIDFromPath(r, "id")
	if err != nil {
		http.Error(w, "invalid campaign id format", http.StatusBadRequest)
		return
	}

	if err := h.campaignService.PauseCampaign(r.Context(), campaignId); err != nil {
		log.Printf("[ERROR] Failed to pause campaign %s: %v", campaignId, err)
		http.Error(w, "internal server error", http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusNoContent)
}

func (h *CampaignHandler) HandleCancelCampaign(w http.ResponseWriter, r *http.Request) {
	campaignId, err := parseUUIDFromPath(r, "id")
	if err != nil {
		http.Error(w, "invalid campaign id format", http.StatusBadRequest)
		return
	}

	r.Body = http.MaxBytesReader(w, r.Body, 1*1024*1024)

	var req CancelCampaignRequest
	decoder := json.NewDecoder(r.Body)
	decoder.DisallowUnknownFields()
	if err := decoder.Decode(&req); err != nil {
		http.Error(w, fmt.Sprintf("invalid request body: %v", err), http.StatusBadRequest)
		return
	}

	if req.Reason == "" {
		http.Error(w, "reason is required", http.StatusBadRequest)
		return
	}

	if err := h.campaignService.CancelCampaign(r.Context(), campaignId, req.Reason); err != nil {
		log.Printf("[ERROR] Failed to cancel campaign %s: %v", campaignId, err)
		http.Error(w, "internal server error", http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusNoContent)
}

func (h *CampaignHandler) HandleDeleteCampaign(w http.ResponseWriter, r *http.Request) {
	campaignId, err := parseUUIDFromPath(r, "id")
	if err != nil {
		http.Error(w, "invalid campaign id format", http.StatusBadRequest)
		return
	}

	if err := h.campaignService.DeleteCampaign(r.Context(), campaignId); err != nil {
		log.Printf("[ERROR] Failed to delete campaign %s: %v", campaignId, err)
		http.Error(w, "internal server error", http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusNoContent)
}

func parseUUIDFromPath(r *http.Request, key string) (uuid.UUID, error) {
	idStr := r.PathValue(key)
	if idStr == "" {
		return uuid.Nil, errors.New("missing path variable")
	}
	return uuid.Parse(idStr)
}
