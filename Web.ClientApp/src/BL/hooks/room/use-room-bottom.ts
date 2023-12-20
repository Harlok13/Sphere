import {useStartGameHub, useStopTimerHub, useToggleReadinessHub} from "../hub-connection/server-methods/server-methods";
import {usePlayerSelector} from "../../slices/player/use-player-selector";
import {
    useGame21PlayersSelector,
    useGameStartedSelector,
    useRoomDataSelector
} from "../../slices/game21/use-game21-selector";
import React from "react";

export const useRoomBottom = () => {
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
        stopTimer
            .invoke(player.roomId, player.id)
            .catch(err => console.error(err.toString()));
    }

    const passHandler = () => {

    }

    const readinessHandler = async () => {
        await toggleReadiness.invoke(player.roomId, player.id).catch(err => console.error(err.toString()));
    }

    const startGameHandler = async () => {
        await startGame
            .invoke(player.roomId, player.id)
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