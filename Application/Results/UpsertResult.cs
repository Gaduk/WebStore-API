namespace Application.Results;

public enum UpsertStatus
{
    Created, Updated
}

public record UpsertResult(UpsertStatus status);