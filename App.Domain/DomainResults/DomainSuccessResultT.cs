namespace App.Domain.DomainResults;

public sealed record DomainSuccessResult<TData>(
    TData Data) : DomainResult(Success: true, IsError: false, IsFailure: false);