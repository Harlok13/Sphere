using App.Contracts.Data;

namespace App.Contracts.Responses;

public sealed record ReconnectingInitRoomDataResponse(
    PlayerDto Player,
    InitRoomDataDto InitRoomData,
    IEnumerable<PlayerDto> Players);