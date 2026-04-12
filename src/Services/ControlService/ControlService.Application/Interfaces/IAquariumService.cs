using Control.Application.DTOs.Aquarium;

namespace Control.Application.Interfaces;

public interface IAquariumService
{
    Task<Guid> CreateAquqariumAsync(
        AquariumRequestDto request, 
        CancellationToken cancellationToken);

    Task DeleteAquariumAsync(
        Guid id, 
        CancellationToken cancellationToken);

    Task<IReadOnlyList<AquariumResponseDto>> GetAllAquariumsAsync(
        AquariumFilterDto filter, 
        int? skip, 
        int? take, 
        CancellationToken cancellationToken);

    Task<AquariumResponseDto> GetAquariumByIdAsync(
        Guid id, 
        CancellationToken cancellationToken);

    Task UpdateAquariumAsync(
        Guid id, 
        AquariumUpdateRequestDto request, 
        CancellationToken cancellationToken);
}