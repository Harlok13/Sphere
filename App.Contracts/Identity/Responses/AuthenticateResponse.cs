namespace App.Contracts.Identity.Responses;

public sealed record AuthenticateResponse(
    Guid PlayerId,
    string PlayerName,
    string Email,
    string Token,
    string RefreshToken,
    PlayerInfoResponse PlayerInfo);