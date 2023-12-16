namespace App.Contracts.Identity.Responses;

public sealed record PlayerInfoResponse(
    Guid Id,
    string AvatarUrl,
    string PlayerName,
    int Matches,
    int Loses,
    int Wins,
    int Draws,
    int AllExp,
    int CurrentExp,
    int TargetExp,
    int Money,
    int Likes,
    int Level,
    int Has21);