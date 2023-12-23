import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {Player} from "../../../contracts/player-response";
import {IAddedCardResponse, ICardDto} from "../../../contracts/added-card-response";
import {produce} from "immer";
import {IUpdatedPlayerIsLeader} from "../../../contracts/updated-player-is-leader";
import {IChangedPlayerReadinessResponse} from "../../../contracts/changed-player-readiness-response";
import {IChangedPlayerMoneyResponse} from "../../../contracts/responses/changed-player-money-response";
import {IChangedPlayerInGameResponse} from "../../../contracts/responses/changed-player-in-game-response";
import {IChangedPlayerMoveResponse} from "../../../contracts/responses/changed-player-move-response";

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
        setIsLeader: (state, action: PayloadAction<IUpdatedPlayerIsLeader>) =>
            produce(state, draft => {
                draft.player.isLeader = action.payload.isLeader;
            }),
        // {
        //     state.player.isLeader = true
        // },
        setReadiness: (state, action: PayloadAction<IChangedPlayerReadinessResponse>) =>
            produce(state, draft => {
                draft.player.readiness = action.payload.readiness;
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
        setMove: (state, action: PayloadAction<IChangedPlayerMoveResponse>) =>
            produce(state, draft => {
                draft.player.move = action.payload.move;
            }),
        // {
        //     state.player.move = action.payload;
        // },
        setGameMoney: (state, action: PayloadAction<IChangedPlayerMoneyResponse>) =>
            produce(state, draft => {
                draft.player.money = action.payload.money;
            }),
        // {
        //     state.player.money = action.payload;
        // },
        setNewCard: (state, action: PayloadAction<IAddedCardResponse>) => {
            return produce(state, draft => {
                draft.player.cards.push(action.payload.cardDto);
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
        setInGame: (state, action: PayloadAction<IChangedPlayerInGameResponse>) => {
            state.player.inGame = action.payload.inGame;
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