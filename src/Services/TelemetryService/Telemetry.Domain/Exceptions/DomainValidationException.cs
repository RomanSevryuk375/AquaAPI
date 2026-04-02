namespace Telemetry.Domain.Exceptions;

public class DomainValidationException(string? message) : Exception(message);
