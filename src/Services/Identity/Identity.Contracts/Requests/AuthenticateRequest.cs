namespace Identity.Contracts.Requests;

public sealed record AuthenticateRequest(
    string Email,
    string Password);