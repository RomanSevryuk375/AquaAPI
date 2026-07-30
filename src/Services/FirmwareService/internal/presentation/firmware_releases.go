package presentation

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"

	"github.com/google/uuid"
	"github.com/romansevryuk375/aquasmart-firmware/internal/application"
)

type RevokeFirmwareRequest struct {
	ErrorMessage string `json:"error_message"`
}

type FirmwareReleasesHandler struct {
	firmwareService *application.FirmwareService
}

func NewFirmwareReleasesHandler(firmwareService *application.FirmwareService) *FirmwareReleasesHandler {
	return &FirmwareReleasesHandler{firmwareService: firmwareService}
}

func (h *FirmwareReleasesHandler) HandleUploadFirmware(w http.ResponseWriter, r *http.Request) {
	r.Body = http.MaxBytesReader(w, r.Body, 10<<20)

	if err := r.ParseMultipartForm(1 << 20); err != nil {
		http.Error(w, "file too large or invalid form", http.StatusBadRequest)
		return
	}

	file, fileHeader, err := r.FormFile("firmware_file")
	if err != nil {
		http.Error(w, "missing 'firmware_file' in form", http.StatusBadRequest)
		return
	}
	defer file.Close()

	hwIdStr := r.FormValue("hardware_id")
	version := r.FormValue("version")
	fileHash := r.FormValue("file_hash")

	hwId, err := uuid.Parse(hwIdStr)
	if err != nil {
		http.Error(w, "invalid hardware_id format", http.StatusBadRequest)
		return
	}

	sizeBytes := int(fileHeader.Size)

	id, err := h.firmwareService.UploadNewRelease(r.Context(), hwId, version, fileHash, sizeBytes, file)
	if err != nil {
		log.Printf("[ERROR] Failed to upload firmware: %v", err)
		http.Error(w, "internal server error", http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusCreated)
	fmt.Fprintf(w, `{"id":"%s"}`, id)
}

func (h *FirmwareReleasesHandler) HandlePublishFirmware(w http.ResponseWriter, r *http.Request) {
	firmwareIdStr := r.PathValue("id")
	if firmwareIdStr == "" {
		http.Error(w, "missing firmware_id parameter", http.StatusBadRequest)
		return
	}

	firmwareId, err := uuid.Parse(firmwareIdStr)
	if err != nil {
		http.Error(w, "invalid firmware_id format", http.StatusBadRequest)
		return
	}

	if err = h.firmwareService.PublishFirmware(r.Context(), firmwareId); err != nil {
		log.Printf("[ERROR] Failed to publish firmware: %v", err)
		http.Error(w, "internal server error", http.StatusInternalServerError)
	}

	w.WriteHeader(http.StatusNoContent)
}

func (h *FirmwareReleasesHandler) HandleRevokeFirmware(w http.ResponseWriter, r *http.Request) {
	r.Body = http.MaxBytesReader(w, r.Body, 1*1024*1024)

	firmwareIdStr := r.PathValue("id")
	if firmwareIdStr == "" {
		http.Error(w, "missing firmware_id parameter", http.StatusBadRequest)
		return
	}

	firmwareId, err := uuid.Parse(firmwareIdStr)
	if err != nil {
		http.Error(w, "invalid firmware_id format", http.StatusBadRequest)
		return
	}

	var req RevokeFirmwareRequest
	decoder := json.NewDecoder(r.Body)
	decoder.DisallowUnknownFields()
	if err := decoder.Decode(&req); err != nil {
		http.Error(w, fmt.Sprintf("invalid request body: %v", err), http.StatusBadRequest)
		return
	}

	if err = h.firmwareService.RevokeFirmware(r.Context(), firmwareId, req.ErrorMessage); err != nil {
		log.Printf("[ERROR] Failed to revoke firmware: %v", err)
		http.Error(w, "internal server error", http.StatusInternalServerError)
	}

	w.WriteHeader(http.StatusNoContent)
}
