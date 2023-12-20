namespace App.Contracts.Responses;

public sealed record ApplicationUserResponse(
    Guid Id,
    string UserName);