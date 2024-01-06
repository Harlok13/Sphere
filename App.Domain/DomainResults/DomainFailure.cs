
namespace App.Domain.DomainResults;

/* custom results should not inherit from this class */
public sealed record DomainFailure(
    string Reason) : DomainResult(Success: false, IsFailure: true, IsError: false);