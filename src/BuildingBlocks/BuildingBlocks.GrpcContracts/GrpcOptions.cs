// Ignore Spelling: Grpc

namespace BuildingBlocks.GrpcContracts;

public sealed record GrpcOptions
{
    public const string SectionName = "GrpcConfiguration";
    public string DeviceServiceUrl { get; set; } = string.Empty;
}
