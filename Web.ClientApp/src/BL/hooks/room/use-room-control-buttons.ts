import {
    useHitHub,
    useStartGameHub, useStayHub,
    useStopTimerHub,
    useToggleReadinessHub
} from "../hub-connection/server-methods/server-methods";
import {usePlayerSelector} from "../../slices/player/use-player-selector";
import {
    useGame21PlayersSelector,
    useGameStartedSelector,
    useRoomDataSelector
} from "../../slices/game21/use-game21-selector";
import React from "react";
import {IToggleReadinessRequest} from "../../../contracts/requests/toggle-readiness-request";
import {IStartGameRequest} from "../../../contracts/requests/start-game-request";
import {IHitRequest} from "../../../contracts/requests/hit-request";
import {IStayRequest} from "../../../contracts/requests/stay-request";
import {IStopTimerRequest} from "../../../contracts/requests/stop-timer-request";

export const useRoomControlButtons = () => {
    const roomData = useRoomDataSelector();
    const player = usePlayerSelector();
    const players = useGame21PlayersSelector();
    const toggleReadiness = useToggleReadinessHub()
    const startGame = useStartGameHub();
    const gameStarted = useGameStartedSelector();
    const stopTimer = useStopTimerHub();
    const hit = useHitHub();
    const stay = useStayHub();

    const getCardHandler = (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        console.log("invoke stop handler")
        const hitRequest: IHitRequest = {
            roomId: player.roomId,
            playerId: player.id
        }

        const stopTimerRequest: IStopTimerRequest = {
            playerId: player.id,
            roomId: player.roomId
        }
        stopTimer
            .invoke(stopTimerRequest)
            .catch(err => console.error(err.toString()));

        hit
            .invoke(hitRequest)
            .catch(err => console.error(err.toString()));
    }

    const passHandler = (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        const stayRequest: IStayRequest = {
            roomId: player.roomId,
            playerId: player.id
        }
        const stopTimerRequest: IStopTimerRequest = {
            playerId: player.id,
            roomId: player.roomId
        }
        stopTimer
            .invoke(stopTimerRequest)
            .catch(err => console.error(err.toString()));

        stay
            .invoke(stayRequest)
            .catch(err => console.error(err.toString()));
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
