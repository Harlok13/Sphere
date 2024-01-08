namespace App.Domain.DomainResults;

public abstract record DomainResult(
    bool Success,
    bool IsFailure,
    bool IsError);