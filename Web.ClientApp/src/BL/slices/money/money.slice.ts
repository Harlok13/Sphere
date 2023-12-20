import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {ISelectStartGameMoneyResponse, SelectStartGameMoney} from "../../../contracts/select-start-game-money-response";

interface MoneyState {
    showModal: boolean;
    selectStartMoney: SelectStartGameMoney;
}

const initialState: MoneyState = {
    showModal: false,
    selectStartMoney: {
        roomId: "",
        lowerBound: 0,
        upperBound: 0,
        availableUpperBound: 0,
        recommendedValue: 0
    }
}

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
        setShowModal: (state, action: PayloadAction<boolean>) => {
            state.showModal = action.payload;
        }
    }
});

export const {
    initSelectStartMoney,
    setRecommendedValue,
    setShowModal,
} = moneySlice.actions;

export default moneySlice.reducer;
