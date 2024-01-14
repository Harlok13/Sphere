using App.Contracts.Data;
using App.Domain.Primitives;

namespace App.Contracts.Responses;

public sealed record ReconnectingInitRoomDataResponse(
    PlayerDto Player,
    InitRoomDataDto InitRoomData,
    List<GameHistoryMessage> GameHistory,
    IEnumerable<PlayerDto> Players);