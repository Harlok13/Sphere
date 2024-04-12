using App.Contracts.Data;
using App.Contracts.Identity.Responses;

namespace App.Contracts.Responses;

public sealed record InitDataResponse(
    PlayerDto? Player,
    PlayerInfoDto? PlayerInfo,
    IEnumerable<PlayerHistoryDto>? PlayerHistories,
    IEnumerable<RoomInLobbyDto>? Rooms);
    