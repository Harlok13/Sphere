import {useGame21PlayersSelector, useGameStartedSelector, useRoomDataSelector} from "store/game21/use-game21-selector";
import {usePlayerSelector} from "store/player/use-player-selector";
import {
    useHitHub,
    useStartGameHub, useStayHub,
    useStopTimerHub,
    useToggleReadinessHub
} from "hooks/hub-connection/server-methods/server-methods";
import {IHitRequest} from "shared/contracts/requests/hit-request";
import {IStopTimerRequest} from "shared/contracts/requests/stop-timer-request";
import {IStayRequest} from "shared/contracts/requests/stay-request";
import React from "react";
import {IToggleReadinessRequest} from "shared/contracts/requests/toggle-readiness-request";
import {IStartGameRequest} from "shared/contracts/requests/start-game-request";


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
