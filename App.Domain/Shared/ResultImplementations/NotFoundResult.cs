namespace App.Domain.Shared.ResultImplementations;

public sealed class NotFoundResult<TData> : Result<TData>
    where TData: class
{
    private NotFoundResult(IEnumerable<Error> errors) : base(isSuccess: false)
        => Errors = (errors as IReadOnlyCollection<Error>)!;

    public override ResultType ResultType => ResultType.NotFound;

    public override IReadOnlyCollection<Error> Errors { get; }
    
    public override TData? Data => default;

    public static NotFoundResult<TData> Create(Error error) => new(new List<Error>(1) { error });

    public static NotFoundResult<TData> Create(IEnumerable<Error> errors) => new(errors);
}