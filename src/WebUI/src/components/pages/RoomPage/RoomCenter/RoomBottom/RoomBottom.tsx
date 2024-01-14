import {useRoomControlButtons} from "hooks/room/control-buttons/use-room-control-buttons";
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
import {useGameHistorySelector} from "store/game21/use-game21-selector";
import {
    GameHistoryMsg
} from "components/shared/pages/main-page/Center/Bottom/room/GameHistory/GameHistoryList/GameHistoryMsg/GameHistoryMsg";


export const RoomBottom = () => {  // TODO: remove redundant arg
    const {roomInfoData, handlers, player, players,} = useRoomControlButtons();

    const gameHistory = useGameHistorySelector();  //TODO reloc
    return (
        <Bottom>
            <ControlPanel>
                {!player.inGame
                    ? (player.readiness
                            ? (<ControlButton disabled={false} clickHandler={handlers.readinessHandler} displayName="Not Ready"/>)
                            : (<ControlButton disabled={false} clickHandler={handlers.readinessHandler} displayName="Ready"/>))
                    : null}

                {player.inGame
                    ? (<>
                        <ControlButton disabled={!player.move} clickHandler={handlers.stayHandler} displayName="Stay"/>
                        <ControlButton disabled={!player.move} clickHandler={handlers.hitHandler} displayName="Hit"/>
                    </>)
                    : players.every(p => p.readiness) && player.isLeader && players.length > 1
                        ? (<ControlButton disabled={false} clickHandler={handlers.startGameHandler} displayName="Start game"/>)
                        : null
                }
            </ControlPanel>
            <GameHistory>
                <GameHistoryTitle/>
                <GameHistoryList>
                    {gameHistory.length > 0 && gameHistory.map(messageData => (<GameHistoryMsg
                        key={v4()}
                        msgData={messageData}
                    />))}
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
