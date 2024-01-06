import React, {ChangeEvent} from "react";
import {SelectStartGameMoney} from "shared/contracts/select-start-game-money-response";
import {useSelectStartMoneySelector, useSelectStartMoneyTypeSelector} from "store/money/use-money-selector";
import {PlayerInfo} from "shared/contracts/player-info-response";
import {usePlayerInfoSelector} from "store/player-info/use-player-info-selector";
import {useNewRoomConfigSelector} from "store/lobby/use-lobby-selector";
import {useDispatch} from "react-redux";
import {useNavigate} from "react-router-dom";
import {useCreateRoomHub, useJoinToRoomHub} from "hooks/hub-connection/server-methods/server-methods";
import {SelectStartMoneyType, setRecommendedValue} from "store/money/money.slice";
import {IJoinToRoomRequest} from "shared/contracts/requests/join-to-room-request";
import {ICreateRoomRequest} from "shared/contracts/requests/create-room-request";
import {IRoomRequest} from "shared/contracts/requests/room-request";
import {setSelectStartMoneyModal} from "store/modals/modals.slice";


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

        // if (response) navigate(NavigateEnum.Room);
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