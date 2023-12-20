using App.Domain.Primitives;

namespace App.Domain.Entities.PlayerInfoEntity;

public sealed partial class PlayerInfo
{
    public event EventHandlerAsync<PlayerInfo>? NotifyMoney;
}