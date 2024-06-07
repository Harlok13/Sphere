using Core.DomainResults;
using GameInteraction.Domain.Enums;

namespace GameInteraction.Domain.Entities.PlayerEntity;

public sealed partial class Player
{
    public void Stay()
    {
        SetMoveStatus(EMoveStatus.Stay);
        SetMove(false);
    }
    
    public DomainResult Hit(Card card)
    {
        var addNewCardResult = AddNewCard(card);
        if (addNewCardResult is DomainError addNewCardError)
            return addNewCardError;
        
        SetMoveStatus(EMoveStatus.Hit);
        SetMove(false);

        return new DomainSuccessResult();
    }
    
    public void EndGame()
    {
        ResetCards();
        SetScore(default);
        ToggleReadiness();
        SetInGame(default);
        SetMoveStatus(EMoveStatus.None);
        
        // TODO: check money, suggest setting a new amount
    }

    public DomainResult StartGame(int startBid, Card card, int delayMs)
    {
        SetInGame(true);
        
        var decreaseMoneyResult = DecreaseMoney(startBid);
        if (decreaseMoneyResult is DomainFailure decreaseMoneyFailure) 
            return decreaseMoneyFailure;
        
        var addNewCardResult = AddNewCard(card: card, delayMs: delayMs);
        if (addNewCardResult is DomainError addNewCardError)
            return addNewCardError;

        return new DomainSuccessResult();
    }
}