namespace App.Domain.Shared;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;

    public Error Error { get; set; }

    public static Result<TValue> Success<TValue>(TValue value) => new(value)

    public static Result Create(bool isSuccess, Error error = default)
    {
        return new Result(isSuccess, error);
    }
}

