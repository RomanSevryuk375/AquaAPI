using Contracts.Exceptions;
using Control.Application.DTOs.Aquarium;
using Control.Application.Interfaces;
using Control.Domain.Entities;
using Control.Domain.Interfaces;
using Control.Domain.SpecificationParams;
using Control.Domain.Specifications;

namespace Control.Application.Services;

public class AquariumService(
    IAquariumRepository aquariumRepository,
    IUnitOfWork unitOfWork) : IAquariumService
{
    public async Task<IReadOnlyList<AquariumResponseDto>> GetAllAquariumsAsync(
        AquariumFilterDto filter,
        int? skip,
        int? take,
        CancellationToken cancellationToken)
    {
        var specification = new AquariumFilterSpecification(
            new AquariumFilterParams
            {
                ControllerId = filter.ControllerId,
                Name = filter.Name,
            });

        var aquariums = await aquariumRepository.GetAllAsync(
            specification,
            skip,
            take,
            cancellationToken);

        return aquariums.Select(x => new AquariumResponseDto
        {
            Id = x.Id,
            Name = x.Name,
            ControllerId = x.ControllerId,
            CreatedAt = x.CreatedAt,
        }).ToList();
    }

    public async Task<AquariumResponseDto> GetAquariumByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var aquarium = await aquariumRepository
            .GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"{nameof(AquariumEntity)} not found");

        return new AquariumResponseDto
        {
            Id = aquarium.Id,
            Name = aquarium.Name,
            ControllerId = aquarium.ControllerId,
            CreatedAt = aquarium.CreatedAt,
        };
    }

    public async Task<Guid> CreateAquariumAsync(
        AquariumRequestDto request,
        CancellationToken cancellationToken)
    {
        var (aquarium, errors) = AquariumEntity.Create(
            request.Name,
            request.ControllerId);

        if (aquarium is null)
        {
            throw new DomainValidationException(
                $"Failed to create {nameof(AquariumEntity)}: {string.Join(", ", errors)}");
        }

        var result = await aquariumRepository.AddAsync(aquarium, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }

    public async Task UpdateAquariumAsync(
        Guid id,
        AquariumUpdateRequestDto request,
        CancellationToken cancellationToken)
    {
        var aquarium = await aquariumRepository
            .GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"{nameof(AquariumEntity)} not found");

        aquarium.SetName(request.Name);

        await aquariumRepository.UpdateAsync(aquarium, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAquariumAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        await aquariumRepository.DeleteAsync(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
