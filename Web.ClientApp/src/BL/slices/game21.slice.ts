import {createSlice} from "@reduxjs/toolkit";

const roomData = [
    {
        playerId: 1,
        playerName: "Harlok",
        readiness: true,
        isLeader: true
    },
    {
        playerId: 2,
        playerName: "Bot",
        readiness: true,
        isLeader: false
    }
]

type Player = {
    playerId: number;
    playerName: string;
    score: number;
    isLeader: boolean;
    readiness: boolean;
    cards: [];
}

const initialState = {
    // guid: "",
    // userScore: 0,
    // opponentScore: 0,
    // gameStart: false,
    // userCards: [],
    // opponentCards: [],
    // readiness: true,
    // isLeader: false,
    inRoom: false,  // TODO: rename?
    // // roomData: {
    // //     guid: "",
    // //     roomSize: 0,
    // //     roomName: "",
    // //     startBid: 0,
    // //     minBid: 0,
    // //     maxBid: 0,
    // //
    // // },
    // player: {
    //     roomGuid: "",
    //     playerId: 0,
    //     playerName: "",
    //     avatar: "",
    //     score: 0,
    //     isLeader: false,
    //     readiness: false,
    //     cards: []
    // },
    players: [],
    gameHistory: [],
    bank: 0
}

export const game21Slice = createSlice({
    name: "game21",
    initialState,
    reducers: {
        // setUserScore: (state, action) => {
        //     state.userScore = action.payload;
        // },
        // setOpponentScore: (state, action) => {
        //     state.opponentScore = action.payload;
        // },
        // setGameStart: (state, action) => {
        //     state.gameStart = action.payload;
        // },
        // setUserCards: (state, action) => {
        //     state.userCards = [...state.userCards, ...action.payload];
        // },
        // setOpponentCards: (state, action) => {
        //     state.opponentCards = [...state.opponentCards, ...action.payload];
        // },
        // setReadiness: (state, action) => {
        //     state.readiness = action.payload;
        // },
        setInRoom: (state, action) => {
            state.inRoom = action.payload;
        },
        setNewPlayer: (state, action) => {
            state.players = [...state.players, action.payload]
            // if (action.payload.playerId !== 0)  // TODO: ahahahah wtf
            //     state.players = [...state.players, action.payload]
        },
        // setPlayerData: (state, action) => {
        //     state.player = action.payload;
        // },
        // removePlayerData: (state) => {
        //     state.player = {
        //         roomGuid: "",
        //         playerId: 0,
        //         playerName: "",
        //         avatar: "",
        //         score: 0,
        //         isLeader: false,
        //         readiness: false,
        //         cards: []
        //     };
        // },
        updatePlayersList: (state, action) => {
            // state.players = [...state.players.filter(p => p.playerId !== action.payload), action.payload];
            state.players = [...action.payload];
        },
        // setNewLeader: (state, action) => {
        //     state.players = [state.players.filter(p => p.playerId !== action.payload.playerId), action.payload];
        // },
        updatePlayerInPlayers: (state, action) => {
            state.players = [...state.players.filter(p => p.playerId !== action.payload.playerId), action.payload.playerId];
        },
        clearPlayers: (state) => {
            state.players = [];
        },
        removePlayerFromPlayers: (state, action) => {
            state.players = [...state.players.filter(p => p.playerId !== action.payload)];
        }
    }
});

export const {
    // setUserScore,
    // setOpponentScore,
    // setGameStart,
    // setUserCards,
    // setOpponentCards,
    // setReadiness,
    setInRoom,
    // setPlayerData,
    // removePlayerData,
    setNewPlayer,
    updatePlayersList,
    // setNewLeader,
    updatePlayerInPlayers,
    clearPlayers,
    removePlayerFromPlayers,

} = game21Slice.actions;

export default game21Slice.reducer;
