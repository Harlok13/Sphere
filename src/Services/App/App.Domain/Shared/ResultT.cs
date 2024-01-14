using System.Collections.ObjectModel;

namespace App.Domain.Shared;

public abstract class Result<TData>
    where TData: class
{
    public abstract ResultType ResultType { get; }
    public abstract IReadOnlyCollection<Error> Errors { get; }
    public abstract TData? Data { get; }
    
    public bool IsSuccess { get; protected init; }

    protected Result(bool isSuccess) => IsSuccess = isSuccess;
}