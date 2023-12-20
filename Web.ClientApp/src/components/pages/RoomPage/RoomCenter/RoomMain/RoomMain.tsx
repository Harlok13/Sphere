import {Main} from "../../../../../shared/pages/main-page/Center/Main/Main";
import {GameArea, TestArea} from "../../../../../shared/pages/main-page/Center/Main/room/GameArea/GameArea";
import {PlayerSide} from "../../../../../shared/pages/main-page/Center/Main/room/GameArea/PlayerSide/PlayerSide";
import {
    PlayerScore
} from "../../../../../shared/pages/main-page/Center/Main/room/GameArea/PlayerSide/PlayerScore/PlayerScore";
import {
    PlayerCardsContainer
} from "../../../../../shared/pages/main-page/Center/Main/room/GameArea/PlayerSide/PlayerCardsContainer/PlayerCardsContainer";
import {useGame21PlayersSelector, useRoomBankSelector} from "../../../../../BL/slices/game21/use-game21-selector";
import {usePlayerSelector} from "../../../../../BL/slices/player/use-player-selector";

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
