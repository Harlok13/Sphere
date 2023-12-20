using App.Domain.Primitives;

namespace App.Domain.Entities.PlayerEntity;

public record CardEventArgs(Card Card) : IEventArgs;

public sealed partial class Player
{
    public event EventHandlerAsync<Player>? NotifyIsLeader;

    public event EventHandlerAsync<Player>? NotifyReadiness;

    public event EventHandlerAsync<Player>? NotifyMoney;

    public event EventHandlerAsync<Player>? NotifyMove;
    
    public event EventHandlerAsync<Player, CardEventArgs>? NotifyCard;
    
    public event EventHandlerAsync<Player>? NotifyCards;
    
    public event EventHandlerAsync<Player>? NotifyInGame;
}