using System.Data;
using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Constants;
using BuildingBlocks.Domain.Results;
using Dapper;
using IdentityService.Application.DTOs;
using IdentityService.Domain.Entities;
using MediatR;

namespace IdentityService.Application.Features.Profile.Queries.GetMyProfile;

public sealed class GetMyProfileHandler(
    ISqlConnectionFactory sqlConnectionFactory) : IRequestHandler<GetMyProfileQuery, Result<UserProfileResponseDto>>
{
    public async Task<Result<UserProfileResponseDto>> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = sqlConnectionFactory.CreateConnection();

        const string SQL = """
            SELECT 
                id AS Id, 
                email AS Email, 
                name AS Name, 
                phone_number AS PhoneNumber, 
                subscription_id AS SubscriptionId, 
                created_at AS CreatedAt
            FROM users
            WHERE id = @UserId
            LIMIT 1
            """;

        UserProfileResponseDto? profile = await connection.QuerySingleOrDefaultAsync<UserProfileResponseDto>(
            SQL, new { request.UserId });

        if (profile is null)
        {
            return Result<UserProfileResponseDto>.Failure(Error.NotFound<User>(
                ErrorMessages.Identity.UserNotFound));
        }

        return Result<UserProfileResponseDto>.Success(profile);
    }
}
