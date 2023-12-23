import {useStartGameHub, useStopTimerHub, useToggleReadinessHub} from "../hub-connection/server-methods/server-methods";
import {usePlayerSelector} from "../../slices/player/use-player-selector";
import {
    useGame21PlayersSelector,
    useGameStartedSelector,
    useRoomDataSelector
} from "../../slices/game21/use-game21-selector";
import React from "react";
import {IToggleReadinessRequest} from "../../../contracts/requests/toggle-readiness-request";
import {IStartGameRequest} from "../../../contracts/requests/start-game-request";

export const useRoomControlButtons = () => {
    const roomData = useRoomDataSelector();
    const player = usePlayerSelector();
    const players = useGame21PlayersSelector();
    const toggleReadiness = useToggleReadinessHub()
    const startGame = useStartGameHub();
    const gameStarted = useGameStartedSelector();
    const stopTimer = useStopTimerHub();

    const getCardHandler = (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        console.log("invoke stop handler")
        // stopTimer
        //     .invoke(player.roomId, player.id)
        //     .catch(err => console.error(err.toString()));
    }

    const passHandler = () => {

    }

    const readinessHandler = async () => {
        const toggleReadinessRequest: IToggleReadinessRequest = {
            roomId: player.roomId,
            playerId: player.id
        }
        await toggleReadiness
            .invoke(toggleReadinessRequest)
            .catch(err => console.error(err.toString()));
    }

    const startGameHandler = async () => {
        const startGameRequest: IStartGameRequest = {
            roomId: player.roomId,
            playerId: player.id
        }
        await startGame
            .invoke(startGameRequest)
            .catch(err => console.error(err.toString()));
    }

    const roomSizeValue = roomData.roomSize;
    // const roomNameValue = game21.roomData.roomName;
    const startBidValue = roomData.startBid;
    const minBidValue = roomData.minBid;
    const maxBidValue = roomData.maxBid;

    const roomInfoData = [
        // {title: "Room name:", value: roomNameValue},
        {title: "Room size", value: roomSizeValue},
        {title: "Start bid", value: startBidValue},
        {title: "Min bid", value: minBidValue},
        {title: "Max bid", value: maxBidValue}
    ];

    const handlers = {
        getCardHandler,
        passHandler,
        readinessHandler,
        startGameHandler,
    }

    return {roomInfoData, handlers, player, players, gameStarted}
}