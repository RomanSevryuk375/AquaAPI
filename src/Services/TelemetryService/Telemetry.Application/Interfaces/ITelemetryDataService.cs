using Contracts.Events.TelemetryEvents;
using Contracts.Results;
using Telemetry.Application.DTOs;

namespace Telemetry.Application.Interfaces;

public interface ITelemetryDataService
{
    Task<ConsumerResult> AddDataAsync(
        TelemetryBatchEvent telemetry,
        CancellationToken cancellationToken);

    Task<IEnumerable<TelemetryDataResponse>> GetAllDataAsync(
        TelemetryDataFilterDto filter,
        int skip,
        int take, 
        CancellationToken cancellationToken);

    Task<TelemetryDataResponse> GetDataByIdAsync(
        Guid id, 
        CancellationToken cancellationToken);
}