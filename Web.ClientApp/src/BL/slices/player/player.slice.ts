import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {Player} from "../../../contracts/player-response";
import {ICardResponse} from "../../../contracts/card-response";
import {produce} from "immer";

export interface PlayerState {
    timer: number;
    player: Player;
}

const initialState: PlayerState = {
    timer: 0,
    player: {
        id: "",
        roomId: "",
        score: 0,
        isLeader: false,
        readiness: false,
        playerName: "",
        avatarUrl: "",
        cards: [],
        move: false,
        money: 0,
        inGame: false
    }
}

const playerSlice = createSlice({
    name: "player",
    initialState,
    reducers: {
        setIsLeader: (state) =>
            produce(state, draft => {
                draft.player.isLeader = true;
            }),
        // {
        //     state.player.isLeader = true
        // },
        setReadiness: (state, action: PayloadAction<boolean>) =>
            produce(state, draft => {
                draft.player.readiness = action.payload;
            }),
        // {
        //     state.player.readiness = action.payload;
        // },
        resetState: (state) =>
            // produce(state, draft => {
            //     draft.player.id = "";
            //     draft.player.roomId = "";
            //     draft.player.score = 0;
            //     draft.player.isLeader = false;
            //     draft.player.readiness = false;
            //     draft.player.playerName = "";
            //     draft.player.avatarUrl = "";
            //     draft.player.cards = [];
            //     draft.player.move = false;
            //     draft.player.money = 0;
            // }),
        {
            // state.player.id = "";
            // state.player.roomId = "";
            // state.player.score = 0;
            // state.player.isLeader = false;
            // state.player.readiness = false;
            // state.player.playerName = "";
            // state.player.avatarUrl = "";
            // state.player.cards = [];
            // state.player.move = false;
            // state.player.money = 0;
            return initialState;
        },
        initPlayerData: (state, action: PayloadAction<Player>) =>
            produce(state, draft => {
                draft.player = action.payload;
            }),
        // {
        //     state.player = action.payload;
        // },
        setMove: (state, action: PayloadAction<boolean>) =>
            produce(state, draft => {
                draft.player.move = action.payload;
            }),
        // {
        //     state.player.move = action.payload;
        // },
        setGameMoney: (state, action: PayloadAction<number>) =>
            produce(state, draft => {
                draft.player.money = action.payload;
            }),
        // {
        //     state.player.money = action.payload;
        // },
        setNewCard: (state, action: PayloadAction<ICardResponse>) => {
            return produce(state, draft => {
                draft.player.cards.push(action.payload);
            });

            // state.player = newState.player;
        },
        setTimer: (state, action: PayloadAction<number>) => {
            return produce(state, draft => {
                draft.timer = action.payload;
                // return draft.timer;
            });
            // state.timer = newState.timer;
        },
        setInGame: (state, action: PayloadAction<boolean>) => {
            state.player.inGame = action.payload;
        }
    }
});

export const {
    resetState,
    setIsLeader,
    setReadiness,
    initPlayerData,
    setMove,
    setGameMoney,
    setNewCard,
    setTimer,
    setInGame,
} = playerSlice.actions;

export default playerSlice.reducer;