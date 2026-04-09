using Device.Application.DTOs.Relay;

namespace Device.Application.Interfaces;

public interface IRelayService
{
    Task<IReadOnlyList<RelayResponseDto>> GetAllRelaysAsync(
        RelayFilterDto filter,
        int? skip,
        int? take,
        CancellationToken cancellationToken);

    Task<RelayResponseDto> GetRelayByIdAsync(
        Guid id, 
        CancellationToken cancellationToken);

    Task<Guid> AddRelayAsync(
        RelayRequestDto request, 
        CancellationToken cancellationToken);

    Task UpdateRelayAsync(
        Guid id,
        RelayUpdateRequestDto updateRequestDto,
        CancellationToken cancellationToken);

    Task DeleteRelayAsync(
        Guid id, 
        CancellationToken cancellationToken);

    Task<bool> ToggleRelayStateAsync(
        Guid id, 
        CancellationToken cancellationToken);

    Task<bool> ToggleRelayModeAsync(
        Guid id, 
        CancellationToken cancellationToken);

    Task<bool> SetRelayStateAsync(
        Guid id, 
        bool state, 
        CancellationToken cancellationToken);
}
