import {createSlice, PayloadAction} from "@reduxjs/toolkit";

export type PlayerActionModal = {
    playerId: string;
    showModal: boolean;
}

interface ModalsState {
    selectStartMoneyModal: boolean;
    reconnectToRoomModal: boolean;
    playerActionModal: PlayerActionModal;
}


const initialState: ModalsState = {
    selectStartMoneyModal: false,
    reconnectToRoomModal: false,
    playerActionModal: {
        playerId: "",
        showModal: false
    },
}

export const modalsSlice = createSlice({
    name: "modal",
    initialState: initialState,
    reducers: {
        setSelectStartMoneyModal: (state, action: PayloadAction<boolean>) => {
            state.selectStartMoneyModal = action.payload;
        },
        setReconnectToRoomModal: (state, action: PayloadAction<boolean>) => {
            state.reconnectToRoomModal = action.payload;
        },
        setPlayerActionModal: (state, action: PayloadAction<PlayerActionModal>) => {
            state.playerActionModal = action.payload;
        },
        resetModalsState: () => {
            return initialState;
        }
    }
});

export const {
    setSelectStartMoneyModal,
    setReconnectToRoomModal,
    setPlayerActionModal,
    resetModalsState,
} = modalsSlice.actions;

export default modalsSlice.reducer