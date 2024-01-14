using App.Domain.Enums;
using App.Domain.Primitives;

namespace App.Contracts.Responses.RoomResponses;

public sealed record AddedGameHistoryMessageResponse(
    string Type,
    string CurrentTime,
    string Message,
    string? PlayerName = default);