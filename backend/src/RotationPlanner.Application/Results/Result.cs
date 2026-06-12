namespace RotationPlanner.Application.Results;

public sealed record Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public ResultError? Error { get; }

    private Result(bool isSuccess, ResultError? error = null)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true);
    public static Result Failure(ResultError error) => new(false, error);

    public static Result Failure(ErrorType type, string code, string message, string? details = null)
    {
        return Failure(new ResultError(type, code, message, details));
    }
}

public sealed record Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public ResultError? Error { get; }
    public T? Value { get; }

    private Result(bool isSuccess, ResultError? error = null, T? value = default)
    {
        IsSuccess = isSuccess;
        Error = error;
        Value = value;
    }

    public static Result<T> Success(T value) => new(true, null, value);
    public static Result<T> Failure(ResultError error) => new(false, error, default);

    public static Result<T> Failure(ErrorType type, string code, string message, string? details = null)
    {
        return Failure(new ResultError(type, code, message, details));
    }
}