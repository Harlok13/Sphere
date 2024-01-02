
namespace App.Domain.DomainResults;

public sealed record DomainFailure(
    string Reason) : DomainResult(Success: false, IsFailure: true, IsError: false);