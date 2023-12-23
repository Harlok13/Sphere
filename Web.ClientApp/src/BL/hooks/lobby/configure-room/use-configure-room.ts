import React, {ChangeEvent} from "react";
import {
    setHighBid,
    setLowBid,
    setMaxBid,
    setMediumBid,
    setMinBid,
    setRoomName,
    setRoomSize,
    setStartBid
} from "../../../slices/lobby/lobby.slice";
import {useDispatch} from "react-redux";
import {useNavigate} from "react-router-dom";
import {useSelectStartGameMoneyHub} from "../../hub-connection/server-methods/server-methods";
import {useNewRoomConfigSelector} from "../../../slices/lobby/use-lobby-selector";
import {usePlayerInfoSelector} from "../../../slices/player-info/use-player-info-selector";
import {RoomSize} from "../../../../constants/configure-room-constants";
import {IRoomRequest} from "../../../../contracts/requests/room-request";
import {ISelectStartGameMoneyRequest} from "../../../../contracts/requests/select-start-game-money-request";
import {SelectStartMoneyType, setType} from "../../../slices/money/money.slice";


export type LobbyPanelHandlers = {
    minBidHandler: (e: ChangeEvent<HTMLInputElement>) => void;
    maxBidHandler: (e: ChangeEvent<HTMLInputElement>) => void;
    startBidHandler: (e: ChangeEvent<HTMLInputElement>) => void;
    roomSizeHandler: (e: ChangeEvent<HTMLInputElement>) => void;
    roomNameHandler: (e: ChangeEvent<HTMLInputElement>) => void;
    createRoomHandler: (e: React.MouseEvent<HTMLButtonElement>) => Promise<void>;
    lowBidHandler: (e: React.MouseEvent<HTMLButtonElement>) => void;
    mediumBidHandler: (e: React.MouseEvent<HTMLButtonElement>) => void;
    highBidHandler: (e: React.MouseEvent<HTMLButtonElement>) => void;
}

export const useConfigureRoom = () => {
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const newRoomData = useNewRoomConfigSelector();
    const playerInfo = usePlayerInfoSelector();
    const newRoomConfig = useNewRoomConfigSelector();

    const selectStartMoney = useSelectStartGameMoneyHub();

    const createRoomHandler = async (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        console.log(newRoomData, playerInfo.id, playerInfo.playerName, "inv")
        // const createRoomRequest: ICreateRoomRequest = {
        //     roomRequest: newRoomData as IRoomRequest,
        //     playerId: playerInfo.id,
        //     selectedStartMoney: 39,
        //     upperBound: 23,
        //     lowerBound: 35
        // }
        // await createRoom
        //     .invoke(createRoomRequest)
        //     .catch(err => console.error(err.toString()));
        const selectStartGameMoneyRequest: ISelectStartGameMoneyRequest = {
            roomRequest: newRoomConfig as IRoomRequest,
            playerId: playerInfo.id
        }
        await selectStartMoney
            .invoke(selectStartGameMoneyRequest)
            .catch(err => console.error(err.toString()));

        dispatch(setType(SelectStartMoneyType.Create))

        // await
        // navigate(NavigateEnum.Room);  // TODO: check response. if not ok -> prohibit navigate
    }

    const minBidHandler = (e: ChangeEvent<HTMLInputElement>) => {
        dispatch(setMinBid(parseInt(e.target.value)));
    }

    const maxBidHandler = (e: ChangeEvent<HTMLInputElement>) => {
        dispatch(setMaxBid(parseInt(e.target.value)));
    }

    const startBidHandler = (e: ChangeEvent<HTMLInputElement>) => {
        dispatch(setStartBid(parseInt(e.target.value)));
    }

    const roomSizeHandler = (e: ChangeEvent<HTMLInputElement>) => {
        let val = parseInt(e.target.value);
        if (isNaN(val)) val = 2;

        val = val > RoomSize.MAX_SIZE
            ? RoomSize.MAX_SIZE
            : val < RoomSize.MIN_SIZE
                ? RoomSize.MIN_SIZE
                : val;

        dispatch(setRoomSize(val));
    }

    const roomNameHandler = (e: ChangeEvent<HTMLInputElement>) => {
        dispatch(setRoomName(e.target.value));
    }

    const lowBidHandler = (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        dispatch(setLowBid())
    }

    const mediumBidHandler = (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        dispatch(setMediumBid());
    }

    const highBidHandler = (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        dispatch(setHighBid());
    }

    const handlers: LobbyPanelHandlers = {
        minBidHandler,
        maxBidHandler,
        startBidHandler,
        roomSizeHandler,
        roomNameHandler,
        createRoomHandler,
        lowBidHandler,
        mediumBidHandler,
        highBidHandler
    }

    return {handlers, newRoomData}
}
