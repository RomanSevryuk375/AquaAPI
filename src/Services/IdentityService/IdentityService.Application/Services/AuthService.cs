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
    IRefreshTokenRepository refreshTokenRepository,
    IPublishEndpoint publishEndpoint,
    IJwtProvider jwtProvider,
    IUnitOfWork unitOfWork,
    IUserContext userContext) : IAuthService
{
    public async Task<LoginResponseDto> RegisterUserAsync(
        RegisterUserRequestDto registerDto,
        CancellationToken cancellationToken)
    {
        var existingUser = await userManager.FindByEmailAsync(registerDto.Email);

        if (existingUser is not null)
        {
            throw new EmailIsBusyException($"{registerDto.Email} is busy.");
        }

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

        var subscription = await subscriptionRepository
            .GetByIdAsync(user.SubscriptionId, cancellationToken);

        var permissions = subscription?.Permissions ?? [];

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

        var accessToken = jwtProvider.GenerateToken(user, permissions);

        var refreshTokenEntity = RefreshTokenEntity
            .Create(user.Id, jwtProvider.GenerateRefreshToken());

        var refreshToken = await refreshTokenRepository
            .AddTokenAsync(refreshTokenEntity, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
    }

    public async Task<LoginResponseDto> LoginAsync(
        LoginUserRequestDto loginUser,
        CancellationToken cancellationToken)
    {
        var existingUser = await userManager
            .FindByEmailAsync(loginUser.Email)
            ?? throw new NotFoundException($"{nameof(UserEntity)} " +
                             $"with email {loginUser.Email} not found");

        var subscription = await subscriptionRepository
            .GetByIdAsync(existingUser!.SubscriptionId, cancellationToken);

        var permissions = subscription?.Permissions ?? [];

        bool isPasswordCorrect = await userManager
            .CheckPasswordAsync(existingUser, loginUser.Password);

        if (!isPasswordCorrect)
        {
            throw new InvalidCredentialsException("Invalid password.");
        }

        var accessToken = jwtProvider
            .GenerateToken(existingUser, permissions);

        var refreshTokenEntity = RefreshTokenEntity
            .Create(existingUser.Id, jwtProvider.GenerateRefreshToken());

        var refreshToken = await refreshTokenRepository
            .AddTokenAsync(refreshTokenEntity, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
    }

    public async Task<LoginResponseDto> LoginWithRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken)
    {
        var usedToken = await refreshTokenRepository
            .GetByTokenStringAsync(refreshToken, cancellationToken);

        if (usedToken is null ||
            usedToken.IsUsed ||
            usedToken.IsRevoked ||
            usedToken.ExpiredAt < DateTime.UtcNow)
        {
            throw new InvalidCredentialsException("Invalid or expired refresh token.");
        }

        usedToken.MarkAsUsed();

        await refreshTokenRepository
            .UpdateTokenAsync(usedToken, cancellationToken);

        var existingUser = await userManager
            .FindByIdAsync(usedToken.UserId.ToString())
            ?? throw new NotFoundException($"{nameof(UserEntity)} " +
                            $"with id {usedToken.Id} not found");

        var subscription = await subscriptionRepository
            .GetByIdAsync(existingUser!.SubscriptionId, cancellationToken);

        var permissions = subscription?.Permissions ?? [];

        var accessToken = jwtProvider
            .GenerateToken(existingUser, permissions);

        var refreshTokenEntity = RefreshTokenEntity
            .Create(existingUser.Id, jwtProvider.GenerateRefreshToken());

        var newRefreshToken = await refreshTokenRepository
            .AddTokenAsync(refreshTokenEntity, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
        };
    }

    public async Task LogoutAsync(CancellationToken cancellationToken)
    {
        await refreshTokenRepository
            .DeleteTokensByUserIdAsync(userContext.UserId, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
