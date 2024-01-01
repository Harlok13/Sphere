import {
    useCancelReconnectingToRoomHub,
    useConfirmReconnectingToRoomHub
} from "BL/hooks/hub-connection/server-methods/server-methods";
import React from "react";
import {useNavigate} from "react-router-dom";
import {NavigateEnum} from "constants/navigate.enum";
import {useDispatch} from "react-redux";
import {setReconnectToRoomModal} from "BL/slices/modals/modals.slice";
import {resetPlayerState} from "../../../slices/player/player.slice";

export type ReconnectToRoomHandlers = {
    confirmReconnectingToRoom: (e: React.MouseEvent<HTMLButtonElement>) => Promise<void>;
    cancelReconnectingToRoomHandler: (e: React.MouseEvent<HTMLButtonElement>) => void;
}

export const useReconnectToRoom = () => {
    const confirmReconnectingToRoom = useConfirmReconnectingToRoomHub();
    const cancelReconnectingToRoom = useCancelReconnectingToRoomHub();

    const navigate = useNavigate();
    const dispatch = useDispatch();

    const confirmReconnectingToRoomHandler = async (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();

        const response = await confirmReconnectingToRoom
            .invoke(5)
            .catch(err => console.error(err.toString()));
        if (response){
            navigate(NavigateEnum.Room);
        }
    }

    const cancelReconnectingToRoomHandler = async (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();

        const response = await cancelReconnectingToRoom
            .invoke(5)
            .catch(err => console.error(err.toString()));

        if (response){
            dispatch(setReconnectToRoomModal(false));
            dispatch(resetPlayerState());
        }
    }

    return {
        confirmReconnectingToRoom: confirmReconnectingToRoomHandler,
        cancelReconnectingToRoomHandler: cancelReconnectingToRoomHandler
    }
}