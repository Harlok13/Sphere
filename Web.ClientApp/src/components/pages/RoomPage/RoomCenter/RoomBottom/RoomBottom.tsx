import {useRoomControlButtons} from "hooks/room/use-room-control-buttons";
import {Bottom} from "components/shared/pages/main-page/Center/Bottom/Bottom";
import {ControlPanel} from "components/shared/pages/main-page/Center/Bottom/room/ControlPanel/ControlPanel";
import {
    ControlButton
} from "components/shared/pages/main-page/Center/Bottom/room/ControlPanel/ControlButton/ControlButton";
import {GameHistory} from "components/shared/pages/main-page/Center/Bottom/room/GameHistory/GameHistory";
import {
    GameHistoryTitle
} from "components/shared/pages/main-page/Center/Bottom/room/GameHistory/GameHistoryTitle/GameHistoryTitle";
import {
    GameHistoryList
} from "components/shared/pages/main-page/Center/Bottom/room/GameHistory/GameHistoryList/GameHistoryList";
import React from "react";
import {
    AboutRoomTitle
} from "components/shared/pages/main-page/Center/Bottom/room/AboutRoom/AboutRoomTitle/AboutRoomTitle";
import {AboutRoom} from "components/shared/pages/main-page/Center/Bottom/room/AboutRoom/AboutRoom";
import {
    AboutRoomList
} from "components/shared/pages/main-page/Center/Bottom/room/AboutRoom/AboutRoomList/AboutRoomList";
import {
    AboutRoomItem
} from "components/shared/pages/main-page/Center/Bottom/room/AboutRoom/AboutRoomList/AboutRoomItem/AboutRoomItem";
import {NotificationPanel} from "components/shared/pages/main-page/Center/Bottom/NotificationPanel/NotificationPanel";
import {v4} from "uuid";


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