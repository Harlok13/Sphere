using System.Collections.ObjectModel;

namespace App.Domain.Shared;

public class Result
{
    public IReadOnlyCollection<Error>? Errors { get; }
    
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    private Result(bool isSuccess, IEnumerable<Error> errors)
    {
        IsSuccess = isSuccess;
        Errors = new ReadOnlyCollection<Error>(new List<Error>(errors));
    }

    private Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Errors = new ReadOnlyCollection<Error>(new List<Error>(1) { error });
    }

    private Result() => IsSuccess = true;

    public static Result Create(bool isSuccess, Error error) => new(isSuccess, error);

    public static Result Create(bool isSuccess, IEnumerable<Error> errors) => new(isSuccess, errors);

    public static Result CreateSuccess() => new();
}