import {useSelectStartGameMoneyHub} from "../../hub-connection/server-methods/server-methods";
import React from "react";
import {useNewRoomConfigSelector, useRoomsSelector} from "../../../slices/lobby/use-lobby-selector";
import {usePlayerInfoSelector} from "../../../slices/player-info/use-player-info-selector";
import {Room} from "../../../../contracts/room-in-lobby-dto";
import {PlayerInfo} from "../../../../contracts/player-info-response";
import {ISelectStartGameMoneyRequest} from "../../../../contracts/requests/select-start-game-money-request";
import {IRoomRequest} from "../../../../contracts/requests/room-request";
import {useDispatch} from "react-redux";
import {SelectStartMoneyType, setType} from "../../../slices/money/money.slice";


export type JoinToRoomHandler = (e: React.MouseEvent<HTMLButtonElement>, roomData: Room) => Promise<void>;

export const useRoomsList = () => {
    const rooms: Array<Room> = useRoomsSelector();
    const playerInfo: PlayerInfo = usePlayerInfoSelector();
    const newRoomConfig = useNewRoomConfigSelector();

    // const joinToRoom = useJoinToRoomHub();
    const selectStartMoney = useSelectStartGameMoneyHub();

    // const navigate = useNavigate();

    const dispatch = useDispatch();

    const joinToRoomHandler: JoinToRoomHandler = async (e: React.MouseEvent<HTMLButtonElement>, roomData: Room): Promise<void> => {
        e.preventDefault();

        // await joinToRoom.invoke(roomId, playerInfo.id, playerInfo.playerName);
        const selectStartGameMoneyRequest: ISelectStartGameMoneyRequest = {
            roomRequest: newRoomConfig as IRoomRequest,
            playerId: playerInfo.id,
            roomId: roomData.id
        }
        await selectStartMoney
            .invoke(selectStartGameMoneyRequest)
            .catch(err => console.error(err.toString()));

        dispatch(setType(SelectStartMoneyType.Join));
        // navigate(NavigateEnum.Room);  // TODO: check response. if not ok -> prohibit navigate
    }

    return {rooms, joinToRoomHandler};
}
