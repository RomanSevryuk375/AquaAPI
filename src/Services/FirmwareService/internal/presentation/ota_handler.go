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

// @Summary      Проверка обновлений (OTA)
// @Description  Ищет активную кампанию обновления для переданного контроллера. Возвращает ссылку на скачивание бинарника из S3.
// @Tags         Device API
// @Produce      json
// @Param        controller_id query string true "UUID контроллера"
// @Success      200 {object} OTAResponse "Возвращает статус обновления и метаданные"
// @Failure      400 {string} string "Неверный формат controller_id"
// @Failure      500 {string} string "Внутренняя ошибка сервера"
// @Router       /api/firmware/v1/ota/check [get]
func (h *OTAHandler) CheckUpdate(w http.ResponseWriter, r *http.Request) {
	// Исправлено: читаем из строки запроса (Query), а не из пути (Path)
	controllerIdStr := r.URL.Query().Get("controller_id")
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

// @Summary      Обновить статус прошивки устройства
// @Description  Вызывается ESP32 для сообщения о начале скачивания, успешной прошивке или ошибке обновления.
// @Tags         Device API
// @Accept       json
// @Produce      json
// @Param        request body UpdateTargetStatusRequest true "Данные о статусе обновления"
// @Success      204 "Статус успешно обновлен"
// @Failure      400 {string} string "Неверный формат запроса"
// @Failure      500 {string} string "Внутренняя ошибка сервера"
// @Router       /api/firmware/v1/ota/status [post]
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
		log.Printf("[ERROR] Failed to update target %s: %v", req.ControllerId, err)
		http.Error(w, "internal server error", http.StatusInternalServerError)
		return
	}

	w.WriteHeader(http.StatusNoContent)
}
