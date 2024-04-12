namespace Identity.Contracts.Requests;

public sealed record RegisterRequest(
    string Email,
    string UserName,
    string Password,
    string PasswordConfirm);