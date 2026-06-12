namespace RotationPlanner.Application.Results;

public sealed record ResultError(ErrorType Type, string Code, string Message, string? Details = null);

