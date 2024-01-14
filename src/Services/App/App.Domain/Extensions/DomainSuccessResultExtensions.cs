using App.Domain.DomainResults;

namespace App.Domain.Extensions;

public static class DomainSuccessResultExtensions
{
    public static bool TryFromDomainResult<TData>(this DomainResult domainResult, out TData? data, out DomainError? error) 
    {
        if (domainResult is DomainSuccessResult<TData> domainSuccessResult)
        {
            data = domainSuccessResult.Data;
            error = default;
            return true;
        }

        data = default;
        error = new DomainError("");
        return false;
    }
}