import {createSlice} from "@reduxjs/toolkit";

const initialState = {
    userId: "",
    avatar: "/img/avatars/default_avatar.png",
    userName: "",
    matches: 0,
    wins: 0,
    loses: 0,
    draws: 0,
    money: 0,
    allExp: 0,
    currentExp: 0,
    targetExp: 0,
    level: 0,
    has21: 0,
    likes: 0,
    history: []
}

export const userInfoSlice = createSlice({
    name: "userInfo",
    initialState,
    reducers: {
        setUserId: (state, action) => {
            state.userId = action.payload;
        },
        setUserName: (state, action) => {
            state.userName = action.payload;
        },
        setMatches: (state, action) => {
            state.matches = action.payload;
        },
        setWins: (state, action) => {
            state.wins = action.payload;
        },
        setLoses: (state, action) => {
            state.loses = action.payload;
        },
        setDraws: (state, action) => {
            state.draws = action.payload;
        },
        setMoney: (state, action) => {
            state.money = action.payload;
        },
        setExp: (state, action) => {
            state.allExp = action.payload;
        },
        setLevel: (state, action) => {
            state.level = action.payload;
        },
        setHas21: (state, action) => {
            state.has21 = action.payload;
        },
        setLikes: (state, action) => {
            state.likes = action.payload;
        },

        setAllStats: (state, action) => {
            // state.exp = action.payload.exp;
            state.has21 = action.payload.has21;
            state.draws = action.payload.draws;
            state.wins = action.payload.wins;
            state.loses = action.payload.loses;
            state.likes = action.payload.likes;
            state.matches = action.payload.matches;
            state.level = action.payload.level;
            state.money = action.payload.money;
        },
        setHistory: (state, action) => {
            state.history = [...state.history, ...action.payload]
        },

        setUser: (state, action) => {
            const statistic = action.payload.playerStatisticResponse;
            const uInfo = action.payload.userResponse;
            console.log(uInfo);
            const playerHistory = action.payload.playerHistoryPayload;

            state.userName = uInfo.userName;
            state.userId = uInfo.id;
            state.allExp = statistic.allExp;
            state.currentExp = statistic.currentExp;
            state.targetExp = statistic.targetExp;
            state.has21 = statistic.has21;
            state.draws = statistic.draws;
            state.wins = statistic.wins;
            state.loses = statistic.loses;
            state.likes = statistic.likes;
            state.matches = statistic.matches;
            state.level = statistic.level;
            state.money = statistic.money;
            state.history = playerHistory;
        }
    }
});

export const {
    setUserId,
    setUserName,
    setMatches,
    setWins,
    setLoses,
    setDraws,
    setMoney,
    setExp,
    setLevel,
    setHas21,
    setLikes,
    setHistory,
    setAllStats,
    setUser
} = userInfoSlice.actions;

export default userInfoSlice.reducer;