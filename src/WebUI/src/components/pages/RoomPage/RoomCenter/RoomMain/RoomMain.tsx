import {useGame21PlayersSelector, useRoomBankSelector} from "store/game21/use-game21-selector";
import {usePlayerSelector} from "store/player/use-player-selector";
import {Main} from "components/shared/pages/main-page/Center/Main/Main";
import {GameArea, TestArea} from "components/shared/pages/main-page/Center/Main/room/GameArea/GameArea";

export const RoomMain = () => {
    const bank = useRoomBankSelector();
    const player = usePlayerSelector();
    const players = useGame21PlayersSelector();

    const props = {bank: bank, player: player, players: players};
    return (
        <Main>
            <GameArea>
                {/*Bank: {bank}*/}
                {/*<PlayerSide>*/}
                {/*    <PlayerScore score="21"/>*/}
                {/*    <PlayerCardsContainer>*/}
                {/*        {player.playerName}: {player.money}*/}
                {/*    </PlayerCardsContainer>*/}
                {/*</PlayerSide>*/}

                {/*<PlayerSide>*/}
                {/*    <PlayerScore score="18"/>*/}
                {/*    <PlayerCardsContainer>*/}

                {/*    </PlayerCardsContainer>*/}
                {/*</PlayerSide>*/}
                <TestArea props={props}/>
            </GameArea>
        </Main>
    )
}
