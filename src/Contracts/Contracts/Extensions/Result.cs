namespace Contracts.Extensions;

public class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public bool IsRetryable { get; } 

    protected Result(bool success, string? error, bool retryable)
    {
        IsSuccess = success;
        Error = error;
        IsRetryable = retryable;
    }

    public static Result Success()
    {
        return new(true, null, false);
    }

    public static Result Failure(string error, bool retryable = false)
    {
        return new(false, error, retryable);
    }
}
