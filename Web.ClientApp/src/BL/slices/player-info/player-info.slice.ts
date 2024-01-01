import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {IPlayerInfoResponse, PlayerInfo} from "../../../contracts/player-info-response";
import {IPlayerHistoryResponse, PlayerHistory} from "../../../contracts/player-history-response";
import {IChangedPlayerInfoMoneyResponse} from "../../../contracts/responses/changed-player-info-money-response";

export interface PlayerInfoState {
    playerInfo: PlayerInfo
    playerHistory: Array<PlayerHistory>
}

const initialState: PlayerInfoState = {
    playerInfo: {
        id: "",
        avatarUrl: "img/avatars/default_avatar.png",
        playerName: "",
        matches: 0,
        loses: 0,
        wins: 0,
        draws: 0,
        allExp: 0,
        currentExp: 0,
        targetExp: 0,
        money: 0,
        likes: 0,
        level: 0,
        has21: 0,
    },
    playerHistory: []
}

export const playerInfoSlice = createSlice({
    name: "playerInfo",
    initialState,
    reducers: {
        setUserId: (state, action: PayloadAction<string>) => {
            state.playerInfo.id = action.payload;
        },
        setAvatarUrl: (state, action: PayloadAction<string>) => {
            state.playerInfo.avatarUrl = action.payload;
        },
        setUserName: (state, action: PayloadAction<string>) => {
            state.playerInfo.playerName = action.payload;
        },
        setMatches: (state, action: PayloadAction<number>) => {
            state.playerInfo.matches = action.payload;
        },
        setWins: (state, action: PayloadAction<number>) => {
            state.playerInfo.wins = action.payload;
        },
        setLoses: (state, action: PayloadAction<number>) => {
            state.playerInfo.loses = action.payload;
        },
        setDraws: (state, action: PayloadAction<number>) => {
            state.playerInfo.draws = action.payload;
        },
        setMoney: (state, action: PayloadAction<IChangedPlayerInfoMoneyResponse>) => {
            state.playerInfo.money = action.payload.money;
        },
        setAllExp: (state, action: PayloadAction<number>) => {
            state.playerInfo.allExp = action.payload;
        },
        setCurrentExp: (state, action: PayloadAction<number>) => {
            state.playerInfo.currentExp = action.payload;
        },
        setTargetExp: (state, action: PayloadAction<number>) => {
            state.playerInfo.targetExp = action.payload;
        },
        setLevel: (state, action: PayloadAction<number>) => {
            state.playerInfo.level = action.payload;
        },
        setHas21: (state, action: PayloadAction<number>) => {
            state.playerInfo.has21 = action.payload;
        },
        setLikes: (state, action: PayloadAction<number>) => {
            state.playerInfo.likes = action.payload;
        },
        setHistory: (state, action: PayloadAction<IPlayerHistoryResponse>) => {
            state.playerHistory = [...state.playerHistory, action.payload];
        },
        initPlayerHistory: (state, action: PayloadAction<Array<IPlayerHistoryResponse>>) => {
            state.playerHistory = [...action.payload];
        },
        initPlayerInfo: (state, action: PayloadAction<IPlayerInfoResponse>) => {
            state.playerInfo = action.payload;
        }
    }
});

export const {
    setUserId,
    setAvatarUrl,
    setUserName,
    setMatches,
    setWins,
    setLoses,
    setDraws,
    setMoney,
    setAllExp,
    setCurrentExp,
    setTargetExp,
    setLevel,
    setHas21,
    setLikes,
    setHistory,
    initPlayerHistory,
    initPlayerInfo,
} = playerInfoSlice.actions;

export default playerInfoSlice.reducer;