import {ControlPanel} from "../../../../../shared/pages/main-page/Center/Bottom/ControlPanel/ControlPanel";
import {
    ControlButton
} from "../../../../../shared/pages/main-page/Center/Bottom/ControlPanel/ControlButton/ControlButton";
import {GameHistory} from "../../../../../shared/pages/main-page/Center/Bottom/GameHistory/GameHistory";
import {
    GameHistoryTitle
} from "../../../../../shared/pages/main-page/Center/Bottom/GameHistory/GameHistoryTitle/GameHistoryTitle";
import {
    GameHistoryList
} from "../../../../../shared/pages/main-page/Center/Bottom/GameHistory/GameHistoryList/GameHistoryList";
import {AboutRoom} from "../../../../../shared/pages/main-page/Center/Bottom/AboutRoom/AboutRoom";
import {
    AboutRoomTitle
} from "../../../../../shared/pages/main-page/Center/Bottom/AboutRoom/AboutRoomTitle/AboutRoomTitle";
import {
    AboutRoomList
} from "../../../../../shared/pages/main-page/Center/Bottom/AboutRoom/AboutRoomList/AboutRoomList";
import {
    NotificationPanel
} from "../../../../../shared/pages/main-page/Center/Bottom/NotificationPanel/NotificationPanel";
import {Bottom} from "../../../../../shared/pages/main-page/Center/Bottom/Bottom";
import {useDispatch, useSelector} from "react-redux";
import {setReadiness} from "../../../../../BL/slices/player.slice";

export const RoomBottom = () => {
    // @ts-ignore
    const game21 = useSelector(state => state.game21);
    const dispatch = useDispatch();

    const getCardHandler = () => {

    }

    const passHandler = () => {

    }

    return (
        <Bottom>
            <ControlPanel>
                {game21.readiness
                    ? (<ControlButton clickHandler={() => dispatch(setReadiness(false))} displayName="Not Ready"/>)
                    : (<ControlButton clickHandler={() => dispatch(setReadiness(true))} displayName="Ready"/>)}

                {game21.startGame ?
                    (<>
                        <ControlButton clickHandler={getCardHandler} displayName="Pass"/>
                        <ControlButton clickHandler={passHandler} displayName="Get card"/>
                    </>)
                : game21.players.every(p => p.readiness) && game21.isLeader
                        ? (<ControlButton clickHandler={() => dispatch(game21.setStartGame(true))} displayName="Start game"/>)
                        : null
                }
            </ControlPanel>
            <GameHistory>
                <GameHistoryTitle/>
                <GameHistoryList>

                </GameHistoryList>
            </GameHistory>
            <AboutRoom>
                <AboutRoomTitle/>
                <AboutRoomList>

                </AboutRoomList>
            </AboutRoom>
            <NotificationPanel/>
        </Bottom>
    )
}