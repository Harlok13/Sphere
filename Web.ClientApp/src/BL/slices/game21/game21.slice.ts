import {createSlice, current, PayloadAction} from "@reduxjs/toolkit";
import {IInitRoomDataResponse} from "../../../contracts/init-room-data-response";
import {IPlayerResponse, Player} from "../../../contracts/player-response";
import {produce} from "immer";
import {PlayerInGame} from "../../hooks/hub-connection/use-gloabl-hub-connection";


type RoomData = IInitRoomDataResponse;

export interface Game21State {
    inRoom: boolean;
    bank: number;
    gameStarted: boolean;
    roomData: RoomData;
    players: Array<Player>;
    gameHistory: Array<string>;
}

const initialState: Game21State = {
    inRoom: false,
    bank: 0,
    gameStarted: false,
    roomData: {
        id: "",
        roomSize: 0,
        roomName: "",
        startBid: 0,
        minBid: 0,
        maxBid: 0,
    },
    players: [],
    gameHistory: [],
}

export const game21Slice = createSlice({
    name: "game21",
    initialState,
    reducers: {
        initRoomData: (state, action: PayloadAction<IInitRoomDataResponse>) => {
            state.roomData = action.payload
            console.log(current(state));
        },
        setInRoom: (state, action: PayloadAction<boolean>) => {
            state.inRoom = action.payload;
        },
        setNewPlayer: (state, action: PayloadAction<IPlayerResponse>) => {
            const nextState = produce(state, draft => {
                draft.players.push(action.payload);
            })
            // state.players = [...state.players, action.payload]
            state.players = nextState.players;
        },
        updatePlayersList: (state, action: PayloadAction<Array<IPlayerResponse>>) => {
            const nextState = produce(state, draft => {
                draft.players = action.payload;
            });
            // state.players = [...state.players.filter(p => p.playerId !== action.payload), action.payload];
            // state.players = [...action.payload];
            state.players = nextState.players;
        },
        updatePlayerInPlayers: (state, action: PayloadAction<IPlayerResponse>) => {
            const nextState = produce(state, draft => {
                const index = draft.players.findIndex(p => p.id === action.payload.id);
                draft.players[index] = action.payload;
            });
            state.players = nextState.players;
        },
        clearPlayersList: (state) => {
            state.players = [];
        },
        removePlayerFromPlayers: (state, action: PayloadAction<string>) => {
            state.players = [...state.players.filter(p => p.id !== action.payload)];
        },
        updateRoomName: (state, action: PayloadAction<string>) => {
            state.roomData.roomName = action.payload;
        },
        updateBankValue: (state, action: PayloadAction<number>) => {
            state.bank = action.payload;
        },
        resetGame21State: (state) => {
            // state.bank = 0;
            // state.inRoom = false;
            // state.gameStarted = false;
            // state.players = [];
            // state.gameHistory = [];
            // state.roomData = {
            //     id: "",
            //     roomSize: 0,
            //     roomName: "",
            //     startBid: 0,
            //     minBid: 0,
            //     maxBid: 0
            // }
            return initialState;
        },
        setGameStarted: (state, action: PayloadAction<boolean>) => {
            const newState = produce(state, draft => {
                draft.gameStarted = action.payload;
            });
            state.gameStarted = newState.gameStarted;
        },
        // setPlayerInGame: (state, action: PayloadAction<PlayerInGame>) => {
        //     state.players.fil
        // }
    }
});

export const {
    setInRoom,
    initRoomData,
    setNewPlayer,
    updatePlayersList,
    updatePlayerInPlayers,
    clearPlayersList,
    removePlayerFromPlayers,
    updateRoomName,
    updateBankValue,
    resetGame21State,
    setGameStarted,
} = game21Slice.actions;

export default game21Slice.reducer;
