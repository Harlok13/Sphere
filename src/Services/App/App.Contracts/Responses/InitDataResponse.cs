using App.Contracts.Data;
using App.Contracts.Identity.Responses;

namespace App.Contracts.Responses;

public sealed record InitDataResponse(
    PlayerDto? Player,
    PlayerInfoResponse? PlayerInfo,
    IEnumerable<PlayerHistoryResponse>? PlayerHistories,
    IEnumerable<RoomInLobbyDto>? Rooms);
    