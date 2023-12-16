using App.Contracts.Identity.Responses;
using App.Domain.Identity.Entities;

namespace App.Contracts.Responses;

public sealed record InitDataResponse(
    PlayerResponse? PlayerResponse,
    PlayerInfoResponse PlayerInfoResponse,
    IEnumerable<PlayerHistoryResponse>? PlayerHistoriesResponse,
    IEnumerable<RoomResponse>? RoomsResponse);
    