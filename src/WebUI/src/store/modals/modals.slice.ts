import {createSlice, PayloadAction} from "@reduxjs/toolkit";

export type ParticipantActionsModal = {
    playerId: string;
    playerName: string;
    showModal: boolean;
    positionX: number;
    positionY: number;
}

interface ModalsState {
    selectStartMoneyModal: boolean;
    reconnectToRoomModal: boolean;
    participantActionsModal: ParticipantActionsModal;
}


const initialState: ModalsState = {
    selectStartMoneyModal: false,
    reconnectToRoomModal: false,
    participantActionsModal: {
        playerId: "",
        playerName: "",
        showModal: false,
        positionX: 0,
        positionY: 0
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
        setParticipantActionsModal: (state, action: PayloadAction<ParticipantActionsModal>) => {
            state.participantActionsModal = action.payload;
        },
        resetModalsState: () => {
            return initialState;
        }
    }
});

export const {
    setSelectStartMoneyModal,
    setReconnectToRoomModal,
    setParticipantActionsModal,
    resetModalsState,
} = modalsSlice.actions;

export default modalsSlice.reducer