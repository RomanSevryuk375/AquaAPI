using Contracts.Enums;
using Contracts.Exceptions;
using IdentityService.Application.DTOs;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Application.Services;

public class AuthService(
    UserManager<UserEntity> userManager,
    IJwtProvider jwtProvider) : IAuthService
{
    public async Task<string> RegisterUserAsync(RegisterUserRequestDto registerDto)
    {
        var existingUser = await userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser is not null)
        {
            throw new EmailIsBusyException($"{registerDto.Email} is busy.");
        }

        var (user, errors) = UserEntity.Create(
            registerDto.Name,
            registerDto.Email,
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

        var token = jwtProvider.GenerateToken(user);

        return token;
    }

    public async Task<string> CheckLoginAsync(LoginUserRequestDto loginUser)
    {
        var existingUser = await userManager.FindByEmailAsync(loginUser.Email)
            ?? throw new NotFoundException($"{nameof(UserEntity)} with email {loginUser.Email} not found");

        bool isPasswordCorrect = await userManager
            .CheckPasswordAsync(existingUser, loginUser.Password);

        if (!isPasswordCorrect)
        {
            throw new InvalidCredentialsException("Invalid password.");
        }

        var token = jwtProvider.GenerateToken(existingUser);

        return token;
    }
}
