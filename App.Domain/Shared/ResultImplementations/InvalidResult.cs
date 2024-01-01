using System.Collections.ObjectModel;

namespace App.Domain.Shared.ResultImplementations;

public sealed class InvalidResult<TData> : Result<TData>
    where TData: class
{
    private InvalidResult(IEnumerable<Error> errors) : base(isSuccess: false)
        => Errors = new ReadOnlyCollection<Error>(errors as List<Error> ?? new List<Error>());

    public override ResultType ResultType => ResultType.Invalid;

    public override IReadOnlyCollection<Error> Errors { get; }

    public override TData? Data => default;

    public static InvalidResult<TData> Create(Error error) => new(new List<Error>(1) { error });

    public static InvalidResult<TData> Create(IEnumerable<Error> errors) => new(errors);
}