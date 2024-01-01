using System.Collections.ObjectModel;

namespace App.Domain.Shared.ResultImplementations;

public sealed class UnexpectedResult<TData> : Result<TData>
    where TData: class
{
    private UnexpectedResult() : base(isSuccess: false) =>
        Errors = new ReadOnlyCollection<Error>(
            new List<Error>(1) { new("There was an unexpected problem.") });

    public override ResultType ResultType => ResultType.Unexpected;

    public override IReadOnlyCollection<Error> Errors { get; }

    public override TData? Data => default;

    public static UnexpectedResult<TData> Create() => new();
}