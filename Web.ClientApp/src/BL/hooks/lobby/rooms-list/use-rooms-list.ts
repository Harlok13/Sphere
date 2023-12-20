import {useNavigate} from "react-router-dom";
import {NavigateEnum} from "../../../../constants/navigate.enum";
import {useJoinToRoomHub, useSelectStartGameMoneyHub} from "../../hub-connection/server-methods/server-methods";
import React from "react";
import {useRoomsSelector} from "../../../slices/lobby/use-lobby-selector";
import {usePlayerInfoSelector} from "../../../slices/player-info/use-player-info-selector";
import {Room} from "../../../../contracts/room-in-lobby-response";
import {PlayerInfo} from "../../../../contracts/player-info-response";
import {useSelectStartMoney} from "../select-start-money/use-select-start-money";


export type JoinToRoomHandler = (e: React.MouseEvent<HTMLButtonElement>, roomData: Room) => Promise<void>;

export const useRoomsList = () => {
    const rooms: Array<Room> = useRoomsSelector();
    const playerInfo: PlayerInfo = usePlayerInfoSelector();

    // const joinToRoom = useJoinToRoomHub();
    const selectStartMoney = useSelectStartGameMoneyHub();

    // const navigate = useNavigate();

    const joinToRoomHandler: JoinToRoomHandler = async (e: React.MouseEvent<HTMLButtonElement>, roomData: Room): Promise<void> => {
        e.preventDefault();

        // await joinToRoom.invoke(roomId, playerInfo.id, playerInfo.playerName);
        await selectStartMoney
            .invoke(roomData.id, playerInfo.id, roomData.startBid, roomData.minBid, roomData.maxBid)
            .catch(err => console.error(err.toString()));

        // navigate(NavigateEnum.Room);  // TODO: check response. if not ok -> prohibit navigate
    }

    return {rooms, joinToRoomHandler};
}
