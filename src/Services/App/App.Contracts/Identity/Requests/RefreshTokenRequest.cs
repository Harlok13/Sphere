namespace App.Contracts.Identity.Requests;

public sealed record RefreshTokenRequest(
    string? AccessToken,
    string RefreshToken);