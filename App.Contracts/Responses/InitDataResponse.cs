using App.Contracts.Identity.Responses;
using App.Domain.Identity.Entities;

namespace App.Contracts.Responses;

public sealed record InitDataResponse(
    ApplicationUser UserResponse,
    PlayerResponse? PlayerResponse,
    PlayerStatisticResponse PlayerStatisticResponse,
    IEnumerable<PlayerHistoryResponse>? PlayerHistoryResponse,
    IEnumerable<RoomResponse>? RoomResponse);