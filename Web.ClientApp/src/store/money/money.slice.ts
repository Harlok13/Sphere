import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {ISelectStartGameMoneyResponse, SelectStartGameMoney} from "shared/contracts/select-start-game-money-response";

interface MoneyState {
    type: SelectStartMoneyType;
    selectStartMoney: SelectStartGameMoney;
}

export enum SelectStartMoneyType{
    None,
    Create,
    Join
}

const initialState: MoneyState = {
    type: SelectStartMoneyType.None,
    selectStartMoney: {
        roomId: "",
        lowerBound: 0,
        upperBound: 0,
        availableUpperBound: 0,
        recommendedValue: 0
    }
}

// TODO: add resetInitialState
export const moneySlice = createSlice({
    name: "money",
    initialState,
    reducers: {
        initSelectStartMoney: (state, action: PayloadAction<ISelectStartGameMoneyResponse>) => {
            state.selectStartMoney = action.payload;
        },
        setRecommendedValue: (state, action: PayloadAction<number>) => {
            if (state.selectStartMoney.availableUpperBound >= action.payload)
                state.selectStartMoney.recommendedValue = action.payload;
        },
        setType: (state, action: PayloadAction<SelectStartMoneyType>) => {
            state.type = action.payload;
        }  // TODO: also add reset type in useEffect
    }
});

export const {
    initSelectStartMoney,
    setRecommendedValue,
    setType,
} = moneySlice.actions;

export default moneySlice.reducer;
