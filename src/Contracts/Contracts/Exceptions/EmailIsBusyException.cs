namespace Contracts.Exceptions;

public class EmailIsBusyException(string? message) : Exception(message);
