import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {Player} from "shared/contracts/data/player-dto";
import {IChangedPlayerIsLeader} from "shared/contracts/responses/player-responses/changed-player-is-leader-response";
import {produce} from "immer";
import {IChangedPlayerReadinessResponse} from "shared/contracts/responses/player-responses/changed-player-readiness-response";
import {IChangedPlayerMoveResponse} from "shared/contracts/responses/player-responses/changed-player-move-response";
import {IChangedPlayerMoneyResponse} from "shared/contracts/responses/player-responses/changed-player-money-response";
import {IChangedPlayerOnlineResponse} from "shared/contracts/responses/player-responses/changed-player-online-response";
import {IReconnectToRoomResponse} from "shared/contracts/responses/reconnect-to-room-response";
import {IChangedPlayerInGameResponse} from "shared/contracts/responses/player-responses/changed-player-in-game-response";
import {IAddedCardResponse} from "shared/contracts/responses/player-responses/added-card-response";

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
        inGame: false,
        online: false
    }
}


const playerSlice = createSlice({
    name: "player",
    initialState,
    reducers: {
        setIsLeader: (state, action: PayloadAction<IChangedPlayerIsLeader>) =>
            produce(state, draft => {
                draft.player.isLeader = action.payload.isLeader;
            }),
        setReadiness: (state, action: PayloadAction<IChangedPlayerReadinessResponse>) =>
            produce(state, draft => {
                draft.player.readiness = action.payload.readiness;
            }),

        resetPlayerState: () => {
            return initialState;
        },
        initPlayerData: (state, action: PayloadAction<Player>) =>
            produce(state, draft => {
                draft.player = action.payload;
            }),

        setMove: (state, action: PayloadAction<IChangedPlayerMoveResponse>) =>
            produce(state, draft => {
                draft.player.move = action.payload.move;
            }),

        setGameMoney: (state, action: PayloadAction<IChangedPlayerMoneyResponse>) =>
            produce(state, draft => {
                draft.player.money = action.payload.money;
            }),

        setNewCard: (state, action: PayloadAction<IAddedCardResponse>) => {
            return produce(state, draft => {
                draft.player.cards.push(action.payload.cardDto);
            });

        },
        setTimer: (state, action: PayloadAction<number>) => {
            return produce(state, draft => {
                draft.timer = action.payload;
            });
        },
        setInGame: (state, action: PayloadAction<IChangedPlayerInGameResponse>) => {
            state.player.inGame = action.payload.inGame;
        },
        setOnline: (state, action: PayloadAction<IChangedPlayerOnlineResponse>) => {
            state.player.online = action.payload.online;
        },
        setRoomId: (state, action: PayloadAction<IReconnectToRoomResponse>) => {
            state.player.roomId = action.payload.roomId;
        }
    }
});

export const {
    resetPlayerState,
    setIsLeader,
    setReadiness,
    initPlayerData,
    setMove,
    setGameMoney,
    setNewCard,
    setTimer,
    setInGame,
    setOnline,
    setRoomId,
} = playerSlice.actions;

export default playerSlice.reducer;