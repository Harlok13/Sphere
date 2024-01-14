namespace App.Contracts.Identity.Requests;

public sealed record RegisterRequest(
    string Email,
    string UserName,
    string Password,
    string PasswordConfirm);