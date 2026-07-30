package presentation

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"

	"github.com/google/uuid"
	"github.com/romansevryuk375/aquasmart-firmware/internal/application"
)

type CreateProfileRequest struct {
	Name          string `json:"name"`
	BoardRevision string `json:"board_revision"`
}

type UpdateProfileRequest struct {
	Name string `json:"name"`
}

type HardwareProfileHandler struct {
	profileService application.HardwareProfileService
}

func NewHardwareProfileHandler(profileService application.HardwareProfileService) *HardwareProfileHandler {
	return &HardwareProfileHandler{profileService: profileService}
}

func (h *HardwareProfileHandler) HandleCreateProfile(w http.ResponseWriter, r *http.Request) {
	r.Body = http.MaxBytesReader(w, r.Body, 1*1024*1024)

	var req CreateProfileRequest
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
	if req.BoardRevision == "" {
		http.Error(w, "board revision is required", http.StatusBadRequest)
		return
	}

	id, err := h.profileService.CreateProfile(r.Context(), req.Name, req.BoardRevision)
	if err != nil {
		log.Printf("[ERROR] Failed to create hardware profile: %v", err)
		http.Error(w, "internal server error", http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusCreated)
	fmt.Fprintf(w, `{"id":"%s"}`, id)
}

func (h *HardwareProfileHandler) HandleRenameProfile(w http.ResponseWriter, r *http.Request) {
	profileIdStr := r.PathValue("id")
	if profileIdStr == "" {
		http.Error(w, "missing profile_id parameter", http.StatusBadRequest)
		return
	}

	profileId, err := uuid.Parse(profileIdStr)
	if err != nil {
		http.Error(w, "invalid profile_id format", http.StatusBadRequest)
		return
	}

	r.Body = http.MaxBytesReader(w, r.Body, 1*1024*1024)

	var req UpdateProfileRequest
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

	if err := h.profileService.RenameProfile(r.Context(), profileId, req.Name); err != nil {
		log.Printf("[ERROR] Failed to update profile %s: %v", profileId, err)
		http.Error(w, "internal server error", http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusNoContent)
}

func (h *HardwareProfileHandler) HandleProfileDeprecate(w http.ResponseWriter, r *http.Request) {
	profileIdStr := r.PathValue("id")
	if profileIdStr == "" {
		http.Error(w, "missing profile_id parameter", http.StatusBadRequest)
		return
	}

	profileId, err := uuid.Parse(profileIdStr)
	if err != nil {
		http.Error(w, "invalid profile_id format", http.StatusBadRequest)
		return
	}

	if err := h.profileService.DeprecateProfile(r.Context(), profileId); err != nil {
		log.Printf("[ERROR] Failed to вeprecate profile %s: %v", profileId, err)
		http.Error(w, "internal server error", http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusNoContent)
}
