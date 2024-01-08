namespace App.Domain.DomainResults;

public sealed record DomainError(
    string Reason) : DomainResult(Success: false, IsFailure: false, IsError: true);