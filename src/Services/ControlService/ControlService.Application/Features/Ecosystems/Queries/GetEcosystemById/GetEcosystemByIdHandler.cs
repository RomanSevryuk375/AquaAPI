using System.Data;
using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using Control.Domain.Entities;
using Dapper;
using MediatR;

namespace Control.Application.Features.Ecosystems.Queries.GetEcosystemById;

public sealed class GetEcosystemByIdHandler(ISqlConnectionFactory sqlConnectionFactory)
    : IRequestHandler<GetEcosystemByIdQuery, Result<EcosystemDto>>
{
    public async Task<Result<EcosystemDto>> Handle(
        GetEcosystemByIdQuery request,
        CancellationToken cancellationToken)
    {
        using IDbConnection connection = sqlConnectionFactory.CreateConnection();

        const string SQL = """
            SELECT
              id,
              type,
              name,
              volume,
              controller_id,
              created_at
            FROM ecosystems
            WHERE id = @EcosystemId
            AND user_id = @UserId
            """;

        EcosystemDto? ecosystem = await connection.QuerySingleOrDefaultAsync<EcosystemDto>(SQL, new
        {
            request.EcosystemId,
            request.UserId
        });

        if (ecosystem is null)
        {
            return Result<EcosystemDto>.Failure(Error.NotFound<Ecosystem>(
                $"Ecosystem {request.EcosystemId} not found."));
        }

        return Result<EcosystemDto>.Success(ecosystem);
    }
}
