using Contracts.Events.ControllerEvents;
using Contracts.Exceptions;
using Device.Application.DTOs.Controller;
using Device.Application.Interfaces;
using Device.Domain.Entities;
using Device.Domain.Interfaces;
using Device.Domain.SpecificationParams;
using Device.Domain.Specifications;
using MassTransit;

namespace Device.Application.Services;

public class ControllerService(
    IControllerRepository controllerRepository,
    IPublishEndpoint publishEndpoint,
    IUserContext userContext,
    IUnitOfWork unitOfWork,
    IMyHasher myHasher) : IControllerService
{
    public async Task<ControllerRegistredResponseDto> AddControllerAsync(ControllerRequestDto request, CancellationToken cancellationToken)
    {
        var userId = userContext.UserId; 

        var deviceToken = Guid.NewGuid().ToString();


        var (controller, errors) = ControllerEntity.Create(
            userId,
            request.MacAddress,
            myHasher.Generate(deviceToken),
            request.Name,
            request.IsOnline);

        if (errors is not null && errors.Count > 0)
        {
            throw new DomainValidationException(
                $"Failed to create {nameof(ControllerEntity)}: {string.Join(", ", errors)}");
        }

        var result = await controllerRepository.AddAsync(controller!, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ControllerRegistredResponseDto
        {
            ControllerId = result,
            DeviceToken = deviceToken
        };
    }

    public async Task DeleteControllerAsync(Guid id, CancellationToken cancellationToken)
    {
        await controllerRepository.DeleteAsync(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<ControllerResponseDto>> GetAllControllersAsync(
        ControllerFilterDto filter, 
        int? skip, 
        int? take, 
        CancellationToken cancellationToken)
    {
        
        var specification = new ControllerFilterSpecification(
            new ControllerFilterParams
            {
                SearchTerm = filter.SearchTerm,
                IsOnline = filter.IsOnline,
            });

        var controllers = await controllerRepository.GetAllAsync(
            specification, 
            skip, 
            take, 
            cancellationToken);

        return controllers.Select(controller => new ControllerResponseDto 
        {
            Id = controller.Id,
            MacAddress = controller.MacAddress,
            Name = controller.Name,
            IsOnline = controller.IsOnline,
            LastSeenAt = controller.LastSeenAt,
            CreatedAt = controller.CreatedAt,
        }).ToList();
    }

    public async Task<ControllerResponseDto> GetControllerByIdAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var controller = await controllerRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"{nameof(ControllerEntity)} not found");

        return new ControllerResponseDto
        {
            Id = controller.Id,
            MacAddress = controller.MacAddress,
            Name = controller.Name,
            IsOnline = controller.IsOnline,
            LastSeenAt = controller.LastSeenAt,
            CreatedAt = controller.CreatedAt,
        };
    }

    public async Task<ControllerPingResponseDto> PingControllerAsync(Guid id, string deviceToken, CancellationToken cancellationToken)
    {
        var controller = await controllerRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"{nameof(ControllerEntity)} not found");

        var verify = myHasher
            .Verify(deviceToken, controller.DeviceTokenHash);

        if (!verify)
        {
            throw new InvalidCredentialsException("DeviceToken is not verified.");
        }

        controller.RecordPing();

        await controllerRepository.UpdateAsync(controller, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ControllerPingResponseDto();
    }

    public async Task<bool> ToggleControllerStateAsync(Guid id, CancellationToken cancellationToken)
    {
        var controller = await controllerRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"{nameof(ControllerEntity)} not found");

        controller.ToggleState();

        await controllerRepository.UpdateAsync(controller, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (!controller.IsOnline)
        {
            await publishEndpoint.Publish(new ControllerNotOnlineEvent 
            { 
                UserId = controller.Id,
                ControllerId = controller.Id,
                LastSeenAt = controller.LastSeenAt,
            }, cancellationToken);
        }

        return controller.IsOnline;
    }

    public async Task UpdateControllerAsync(
        Guid id,
        ControllerUpdateRequestDto updateRequestDto, 
        CancellationToken cancellationToken)
    {
        var controller = await controllerRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"{nameof(ControllerEntity)} not found");

        var errors = controller.Update(
            updateRequestDto.MacAddress,
            updateRequestDto.Name);

        if (errors is not null && errors.Count > 0)
        {
            throw new DomainValidationException(
                $"Failed to create {nameof(ControllerEntity)}: {string.Join(", ", errors)}");
        }

        await controllerRepository.UpdateAsync(controller, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
