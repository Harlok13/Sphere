import {Main} from "../../../../../shared/pages/main-page/Center/Main/Main";
import {GameArea} from "../../../../../shared/pages/main-page/Center/Main/room/GameArea/GameArea";
import {PlayerSide} from "../../../../../shared/pages/main-page/Center/Main/room/GameArea/PlayerSide/PlayerSide";
import {
    PlayerScore
} from "../../../../../shared/pages/main-page/Center/Main/room/GameArea/PlayerSide/PlayerScore/PlayerScore";
import {
    PlayerCardsContainer
} from "../../../../../shared/pages/main-page/Center/Main/room/GameArea/PlayerSide/PlayerCardsContainer/PlayerCardsContainer";

export const RoomMain = () => {
    return (
        <Main>
            <GameArea>
                <PlayerSide>
                    <PlayerScore score="21"/>
                    <PlayerCardsContainer>

                    </PlayerCardsContainer>
                </PlayerSide>

                <PlayerSide>
                    <PlayerScore score="18"/>
                    <PlayerCardsContainer>

                    </PlayerCardsContainer>
                </PlayerSide>
            </GameArea>
        </Main>
    )
}