import {useSelectStartMoneySelector, useSelectStartMoneyTypeSelector} from "../../../slices/money/use-money-selector";
import React, {ChangeEvent} from "react";
import {useDispatch} from "react-redux";
import {SelectStartMoneyType, setRecommendedValue} from "../../../slices/money/money.slice";
import {SelectStartGameMoney} from "../../../../contracts/select-start-game-money-response";
import {useCreateRoomHub, useJoinToRoomHub} from "../../hub-connection/server-methods/server-methods";
import {usePlayerInfoSelector} from "../../../slices/player-info/use-player-info-selector";
import {PlayerInfo} from "../../../../contracts/player-info-response";
import {useNavigate} from "react-router-dom";
import {NavigateEnum} from "../../../../constants/navigate.enum";
import {IJoinToRoomRequest} from "../../../../contracts/requests/join-to-room-request";
import {ICreateRoomRequest} from "../../../../contracts/requests/create-room-request";
import {IRoomRequest} from "../../../../contracts/requests/room-request";
import {useNewRoomConfigSelector} from "../../../slices/lobby/use-lobby-selector";
import {setSelectStartMoneyModal} from "BL/slices/modals/modals.slice";

export type SelectStartMoneyHandlers = {
    selectStartMoneyHandler: (e: ChangeEvent<HTMLInputElement>) => void;
    confirmHandler: (e: React.MouseEvent<HTMLButtonElement>) => Promise<void>;
    cancelHandler: (e: React.MouseEvent<HTMLButtonElement>) => Promise<void>;
}

export const useSelectStartMoney = () => {
    const selectStartMoney: SelectStartGameMoney = useSelectStartMoneySelector();
    const playerInfo: PlayerInfo = usePlayerInfoSelector();
    const selectStartMoneyType = useSelectStartMoneyTypeSelector();
    const newRoomData = useNewRoomConfigSelector();

    const dispatch = useDispatch();
    const navigate = useNavigate();

    const joinToRoom = useJoinToRoomHub();
    const createRoom = useCreateRoomHub();

    const selectStartMoneyHandler = (e: ChangeEvent<HTMLInputElement>) => {
        dispatch(setRecommendedValue(parseInt(e.target.value)));
    }

    const confirmHandler = async (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();

        let response = false;
        switch (selectStartMoneyType){
            case SelectStartMoneyType.Join:
                const joinToRoomRequest: IJoinToRoomRequest = {
                    roomId: selectStartMoney.roomId,
                    playerId: playerInfo.id,
                    selectedStartMoney: selectStartMoney.recommendedValue
                }
                response = await joinToRoom
                    .invoke(joinToRoomRequest);
                    // .catch(err => console.error(err.toString()));
                break;

            case SelectStartMoneyType.Create:
                const createRoomRequest: ICreateRoomRequest = {
                    roomRequest: newRoomData as IRoomRequest,
                    playerId: playerInfo.id,
                    selectedStartMoney: selectStartMoney.recommendedValue,
                    upperBound: selectStartMoney.upperBound,
                    lowerBound: selectStartMoney.lowerBound
                }
                response = await createRoom
                    .invoke(createRoomRequest);
                    // .catch(err => console.error(err.toString()));
                break;

            default: throw Error();  // TODO: finish
        }

        if (response) navigate(NavigateEnum.Room);
    }

    const cancelHandler = async (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();

        dispatch(setSelectStartMoneyModal(false));
    }

    const handlers: SelectStartMoneyHandlers = {
        selectStartMoneyHandler,
        confirmHandler,
        cancelHandler
    }

    return {selectStartMoney, handlers};
}