namespace RotationPlanner.Application.Results;

public sealed record ResultError(ErrorType Type, string Message, string? Details = null);

