namespace App.Contracts.Identity.Responses;

public sealed record PlayerStatisticResponse(
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