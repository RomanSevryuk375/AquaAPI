namespace Contracts.Exceptions;

public class DomainValidationException(string? message) : Exception(message);
