namespace App.Domain.Shared;

public enum ResultType : byte
{
    Ok,
    Invalid,
    Unauthorized,
    PartialOk,
    NotFound,
    PermissionDenied,
    Unexpected
}