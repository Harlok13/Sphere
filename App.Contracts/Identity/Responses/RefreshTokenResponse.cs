namespace App.Contracts.Identity.Responses;

public sealed record RefreshTokenResponse(
    string AccessToken,
    string RefreshToken);