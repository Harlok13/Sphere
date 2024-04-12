namespace Identity.Contracts.Responses;

public sealed record RefreshTokenResponse(
    string AccessToken,
    string RefreshToken);