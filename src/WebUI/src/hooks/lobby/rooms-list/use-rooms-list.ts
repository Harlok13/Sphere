import {useRoomsSelector} from "store/lobby/use-lobby-selector";
import {Room} from "shared/contracts/room-in-lobby-dto";
import {PlayerInfo} from "shared/contracts/player-info-response";
import {usePlayerInfoSelector} from "store/player-info/use-player-info-selector";
import {useSelectStartGameMoneyHub} from "hooks/hub-connection/server-methods/server-methods";
import {useDispatch} from "react-redux";
import {ISelectStartGameMoneyRequest} from "shared/contracts/requests/select-start-game-money-request";
import {SelectStartMoneyType, setType} from "store/money/money.slice";
import React from "react";


export type JoinToRoomHandler = (e: React.MouseEvent<HTMLButtonElement>, roomData: Room) => Promise<void>;

export const useRoomsList = () => {
    const rooms: Array<Room> = useRoomsSelector();
    const playerInfo: PlayerInfo = usePlayerInfoSelector();

    const selectStartMoney = useSelectStartGameMoneyHub();
    const dispatch = useDispatch();

    const joinToRoomHandler: JoinToRoomHandler = async (e: React.MouseEvent<HTMLButtonElement>, roomData: Room): Promise<void> => {
        e.preventDefault();

        const selectStartGameMoneyRequest: ISelectStartGameMoneyRequest = {
            roomRequest: null,
            playerId: playerInfo.id,
            roomId: roomData.id
        }
        await selectStartMoney
            .invoke(selectStartGameMoneyRequest)
            .catch(err => console.error(err.toString()));

        dispatch(setType(SelectStartMoneyType.Join));
    }

    return {rooms, joinToRoomHandler};
}
