using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using IdentityService.Application.DTOs;

namespace IdentityService.Application.Features.Profile.Queries.GetMyProfile;

public sealed record GetMyProfileQuery : IQuery<Result<UserProfileResponseDto>>;
