using System.Collections.ObjectModel;
using App.Domain.Shared;

namespace App.Application.Extensions;

public static class ResultExtensions
{
    public static bool TryFromResult<TData>(
        this Result<TData> resultT,
        out TData? data,
        out IReadOnlyCollection<Error> errors)
        where TData : class
    {
        switch (resultT.ResultType)
        {
            case ResultType.Ok:
                data = resultT.Data;
                errors = new ReadOnlyCollection<Error>(new List<Error>());
                return resultT.IsSuccess;
            
            default:
                data = default;
                errors = resultT.Errors;
                return resultT.IsSuccess;
        }
    }
}