namespace App.Contracts.Identity.Requests;

public sealed record AuthenticateRequest(
    string Email,
    string Password);