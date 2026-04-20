using Notification.Application.DTOs.MaintenanceLog;

namespace Notification.Application.Interfaces;

public interface IMaintenanceLogService
{
    Task<IReadOnlyList<MaintenanceLogResponseDto>> GetAllLogs(
        MaintenanceLogFilterDto filter,
        int? skip,
        int? take,
        CancellationToken cancellationToken);

    Task<MaintenanceLogResponseDto> GetLogById(
        Guid id,
        CancellationToken cancellationToken);

    Task<Guid> AddLogAsync(
        MaintenanceLogRequestDto request,
        CancellationToken cancellationToken);
}
