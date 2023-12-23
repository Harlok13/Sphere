import {ControlPanel} from "../../../../shared/pages/main-page/Center/Bottom/room/ControlPanel/ControlPanel";
import {
    ControlButton
} from "../../../../shared/pages/main-page/Center/Bottom/room/ControlPanel/ControlButton/ControlButton";
import {GameHistory} from "../../../../shared/pages/main-page/Center/Bottom/room/GameHistory/GameHistory";
import {
    GameHistoryTitle
} from "../../../../shared/pages/main-page/Center/Bottom/room/GameHistory/GameHistoryTitle/GameHistoryTitle";
import {
    GameHistoryList
} from "../../../../shared/pages/main-page/Center/Bottom/room/GameHistory/GameHistoryList/GameHistoryList";
import {AboutRoom} from "../../../../shared/pages/main-page/Center/Bottom/room/AboutRoom/AboutRoom";
import {
    AboutRoomTitle
} from "../../../../shared/pages/main-page/Center/Bottom/room/AboutRoom/AboutRoomTitle/AboutRoomTitle";
import {
    AboutRoomList
} from "../../../../shared/pages/main-page/Center/Bottom/room/AboutRoom/AboutRoomList/AboutRoomList";
import {
    NotificationPanel
} from "../../../../shared/pages/main-page/Center/Bottom/NotificationPanel/NotificationPanel";
import {Bottom} from "../../../../shared/pages/main-page/Center/Bottom/Bottom";
import {
    AboutRoomItem
} from "../../../../shared/pages/main-page/Center/Bottom/room/AboutRoom/AboutRoomList/AboutRoomItem/AboutRoomItem";
import {v4} from "uuid";
import {useRoomControlButtons} from "../../../../BL/hooks/room/use-room-control-buttons";

export const RoomBottom = () => {
    const {roomInfoData, handlers, player, players, gameStarted} = useRoomControlButtons();

    return (
        <Bottom>
            <ControlPanel>
                {!gameStarted
                    ? (player.readiness
                            ? (<ControlButton disabled={false} clickHandler={handlers.readinessHandler} displayName="Not Ready"/>)
                            : (<ControlButton disabled={false} clickHandler={handlers.readinessHandler} displayName="Ready"/>))
                    : null}

                {gameStarted
                    ? (<>
                        <ControlButton disabled={!player.move} clickHandler={handlers.passHandler} displayName="Pass"/>
                        <ControlButton disabled={!player.move} clickHandler={handlers.getCardHandler} displayName="Get card"/>
                    </>)
                    : players.every(p => p.readiness) && player.isLeader && players.length > 1
                        // ? (<ControlButton clickHandler={() => game21.setStartGame(true)} displayName="Start game"/>)
                        ? (<ControlButton disabled={false} clickHandler={handlers.startGameHandler} displayName="Start game"/>)
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
                    {roomInfoData.map(raw => (<AboutRoomItem key={v4()} roomInfoData={raw}/>))}
                </AboutRoomList>
            </AboutRoom>
            <NotificationPanel/>
        </Bottom>
    )
}