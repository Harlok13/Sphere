using System.Collections.ObjectModel;

namespace App.Domain.Shared.ResultImplementations;

public sealed class SuccessResult<TData> : Result<TData>
    where TData: class
{
    private SuccessResult(TData data) : base(isSuccess: true) 
        => Data = data;

    public override ResultType ResultType => ResultType.Ok;

    public override IReadOnlyCollection<Error> Errors => 
        new ReadOnlyCollection<Error>(new List<Error>());

    public override TData Data { get; }

    public static SuccessResult<TData> Create(TData data) => new(data);
}
