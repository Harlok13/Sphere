using App.Contracts.Data;

namespace App.Contracts.Responses.PlayerResponses;

public sealed record CreatedPlayerResponse(
    PlayerDto Player,
    InitRoomDataDto InitRoomData,
    IEnumerable<PlayerDto> Players);