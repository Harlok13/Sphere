namespace App.Domain.DomainResults;

public sealed record DomainSuccessResult() 
    : DomainResult(Success: true, IsFailure: false, IsError: false);