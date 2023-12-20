import {useSelectStartMoneySelector} from "../../../slices/money/use-money-selector";
import React, {ChangeEvent} from "react";
import {useDispatch} from "react-redux";
import {setRecommendedValue} from "../../../slices/money/money.slice";
import {SelectStartGameMoney} from "../../../../contracts/select-start-game-money-response";
import {useJoinToRoomHub} from "../../hub-connection/server-methods/server-methods";
import {usePlayerInfoSelector} from "../../../slices/player-info/use-player-info-selector";
import {PlayerInfo} from "../../../../contracts/player-info-response";
import {useNavigate} from "react-router-dom";
import {NavigateEnum} from "../../../../constants/navigate.enum";

export type SelectStartMoneyHandlers = {
    selectStartMoneyHandler: (e: ChangeEvent<HTMLInputElement>) => void;
    confirmHandler: (e: React.MouseEvent<HTMLButtonElement>) => Promise<void>;
    cancelHandler: (e: React.MouseEvent<HTMLButtonElement>) => Promise<void>;
}

export const useSelectStartMoney = () => {
    const selectStartMoney: SelectStartGameMoney = useSelectStartMoneySelector();
    const playerInfo: PlayerInfo = usePlayerInfoSelector();

    const dispatch = useDispatch();
    const navigate = useNavigate();

    const joinToRoom = useJoinToRoomHub();

    const selectStartMoneyHandler = (e: ChangeEvent<HTMLInputElement>) => {
        dispatch(setRecommendedValue(parseInt(e.target.value)));
    }

    const confirmHandler = async (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();

        await joinToRoom
            .invoke(selectStartMoney.roomId, playerInfo.id, selectStartMoney.recommendedValue)
            .catch(err => console.error(err.toString()));

        navigate(NavigateEnum.Room);
    }

    const cancelHandler = async (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
    }

    const handlers: SelectStartMoneyHandlers = {
        selectStartMoneyHandler,
        confirmHandler,
        cancelHandler
    }

    return {selectStartMoney, handlers};
}