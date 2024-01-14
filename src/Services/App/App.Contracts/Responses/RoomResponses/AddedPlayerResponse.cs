using App.Contracts.Data;

namespace App.Contracts.Responses.RoomResponses;

public sealed record AddedPlayerResponse(
    PlayerDto Player);