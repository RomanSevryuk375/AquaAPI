using Contracts.Enums;
using Contracts.Events.UserEvents;
using Contracts.Exceptions;
using IdentityService.Application.DTOs;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Application.Services;

public class AuthService(
    UserManager<UserEntity> userManager,
    ISubscriptionRepository subscriptionRepository,
    IPublishEndpoint publishEndpoint,
    IJwtProvider jwtProvider) : IAuthService
{
    public async Task<string> RegisterUserAsync(
        RegisterUserRequestDto registerDto, 
        CancellationToken cancellationToken)
    {
        var existingUser = await userManager.FindByEmailAsync(registerDto.Email);

        if (existingUser is not null)
        {
            throw new EmailIsBusyException($"{registerDto.Email} is busy.");
        }

        var subscription = await subscriptionRepository.GetByIdAsync(existingUser!.SubscriptionId, cancellationToken);
        var permissions = subscription?.Permissions ?? [];

        var (user, errors) = UserEntity.Create(
            registerDto.Name,
            registerDto.Email,
            registerDto.PhoneNumber,
            Guid.Parse(SubscriptionEnum.Free));

        if (user is null)
        {
            throw new DomainValidationException(
                $"Failed to create {nameof(UserEntity)}: {string.Join(", ", errors)}");
        }

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            var error = string.Join(", ", result.Errors.Select(x => x.Description));
            throw new RegisterException(
                $"Failed to register user {user.Id}: {string
                .Join(", ", result.Errors.Select(x => x.Description))}");
        }

        await publishEndpoint.Publish(new UserCreatedEvent
        {
            UserId = user.Id,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber!,
            CreatedAt = user.CreatedAt,
        }, cancellationToken);

        var token = jwtProvider.GenerateToken(user, permissions);

        return token;
    }

    public async Task<string> CheckLoginAsync(LoginUserRequestDto loginUser, CancellationToken cancellationToken)
    {
        var existingUser = await userManager.FindByEmailAsync(loginUser.Email)
            ?? throw new NotFoundException($"{nameof(UserEntity)} with email {loginUser.Email} not found");

        var subscription = await subscriptionRepository.GetByIdAsync(existingUser!.SubscriptionId, cancellationToken);
        var permissions = subscription?.Permissions ?? [];

        bool isPasswordCorrect = await userManager
            .CheckPasswordAsync(existingUser, loginUser.Password);

        if (!isPasswordCorrect)
        {
            throw new InvalidCredentialsException("Invalid password.");
        }

        var token = jwtProvider.GenerateToken(existingUser, permissions);

        return token;
    }
}
