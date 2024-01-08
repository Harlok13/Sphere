using App.Domain.Shared;
using App.Domain.Shared.ResultImplementations;

namespace App.Domain.Entities.PlayerInfoEntity;

public sealed partial class PlayerInfo
{
    public sealed record JoinToRoomDto(int Money, string PlayerName);
    
    public Result<JoinToRoomDto> JoinToRoom(int selectedStartMoney)  // TODO: finish
    {
        DecrementMoney(selectedStartMoney);
        return SuccessResult<JoinToRoomDto>.Create(new JoinToRoomDto(selectedStartMoney, PlayerName));
    }
}