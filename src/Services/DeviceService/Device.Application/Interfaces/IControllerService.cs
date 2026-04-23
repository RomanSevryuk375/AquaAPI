using Device.Application.DTOs.Controller;

namespace Device.Application.Interfaces;

public interface IControllerService
{
    Task<List<ControllerResponseDto>> GetAllControllersAsync(
        ControllerFilterDto filter,
        int? skip,
        int? take,
        CancellationToken cancellationToken);

    Task<ControllerResponseDto> GetControllerByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<ControllerRegistredResponseDto> AddControllerAsync(
        ControllerRequestDto request, 
        CancellationToken cancellationToken);

    Task UpdateControllerAsync(
        Guid id,
        ControllerUpdateRequestDto updateRequestDto,
        CancellationToken cancellationToken);

    Task DeleteControllerAsync(
        Guid id, 
        CancellationToken cancellationToken);

    Task<ControllerPingResponseDto> PingControllerAsync(
        Guid id,
        string deviceToken,
        CancellationToken cancellationToken);

    Task<bool> ToggleControllerStateAsync(
        Guid id, 
        CancellationToken cancellationToken);
}
