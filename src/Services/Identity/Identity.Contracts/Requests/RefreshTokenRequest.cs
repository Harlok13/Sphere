namespace Identity.Contracts.Requests;

public sealed record RefreshTokenRequest(
    string? AccessToken,
    string RefreshToken);