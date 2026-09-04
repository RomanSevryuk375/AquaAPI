using System.Data.Common;
using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using Microsoft.Extensions.Options;
using Telemetry.Application.Extensions;
using Telemetry.Application.Features.BackgroundJobs.Commands.CleanUpOldData;

namespace Telemetry.Application.UnitTests.Features.BackgroundJobs;

public class CleanUpOldDataHandlerTests
{
    private readonly ISqlConnectionFactory _sqlConnectionFactoryMock;
    private readonly DbConnection _dbConnectionMock;
    private readonly DbCommand _dbCommandMock;
    private readonly IOptions<TelemetrySettings> _optionsMock;
    private readonly CleanUpOldDataHandler _handler;

    public CleanUpOldDataHandlerTests()
    {
        _sqlConnectionFactoryMock = Substitute.For<ISqlConnectionFactory>();
        _dbConnectionMock = Substitute.For<DbConnection>();
        _dbCommandMock = Substitute.For<DbCommand>();

        _dbConnectionMock.CreateCommand().Returns(_dbCommandMock);
        DbParameterCollection dbParameterCollectionMock = Substitute.For<DbParameterCollection>();
        DbParameter dbParameterMock = Substitute.For<DbParameter>();
        _dbCommandMock.Parameters.Returns(dbParameterCollectionMock);
        _dbCommandMock.CreateParameter().Returns(dbParameterMock);
        dbParameterCollectionMock.Add(Arg.Any<object>()).Returns(0);
        _dbCommandMock.ExecuteNonQueryAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(0));

        _sqlConnectionFactoryMock.CreateConnection().Returns(_dbConnectionMock);

        var settings = new TelemetrySettings
        {
            MaxLiveTimeForRawDataInHours = -24,
            MaxLiveTimeForMinutesDataInDayes = -7,
            MaxLiveTimeForHourseDataInDayes = -30
        };
        _optionsMock = Options.Create(settings);

        _handler = new CleanUpOldDataHandler(_sqlConnectionFactoryMock, _optionsMock);
    }

    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public async Task HandleShouldExecuteSuccessfullyAndCreateConnection()
    {
        // Arrange
        var command = new CleanUpOldDataCommand();

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _sqlConnectionFactoryMock.Received(1).CreateConnection();
    }
}
