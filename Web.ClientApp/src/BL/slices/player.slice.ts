import {createSlice} from "@reduxjs/toolkit";

const playerSlice = createSlice({
    name: "player",
    initialState: {
        // id: 0,
        // userName: "Harlok",
        // score: 0,
        // isLeader: true,
        // readiness: false,
        // cards: [],
        // money: 0
        // playerId: 0,
        id: "",
        roomGuid: "",
        score: 0,
        isLeader: false,
        readiness: false,
        playerName: "",
        avatarUrl: "",
        cards: []
    },
    reducers: {
        // setPlayerId: (state, action) => {
        //     state.playerId = action.payload;
        // },
        setIsLeader: (state) => {
            state.isLeader = true
        },
        setReadiness: (state, action) => {
            state.readiness = action.payload;
        },
        setRoomGuid: (state, action) => {
            state.roomGuid = action.payload
        },
        resetState: (state) => {
            state.roomGuid = "";
            state.score = 0;
            state.isLeader = false;
            state.readiness = false;
            state.cards = [];
        },
        initPlayerData: (state, action) => {
            state.roomGuid = action.payload.roomId;
            state.id = action.payload.id;
            state.score = action.payload.score;
            state.isLeader = action.payload.isLeader;
            state.readiness = action.payload.readiness;
            state.playerName = action.payload.playerName;
            state.avatarUrl = action.payload.avatarUrl;
            state.cards = action.payload.cards;
        }
    }
});

export const {
    // setPlayerId,
    setIsLeader,
    setReadiness,
    initPlayerData,
} = playerSlice.actions;

export default playerSlice.reducer;