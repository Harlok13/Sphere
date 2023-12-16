import {configureStore} from "@reduxjs/toolkit";
import game21Reducer from "./slices/game21/game21.slice";
import playerInfoReducer from "./slices/player-info/player-info.slice";
import lobbyReducer from "./slices/lobby/lobby.slice";
import playerReducer from "./slices/player/player.slice";
import moneyReducer from "./slices/money/money.slice";
import notificationsReducer from "./slices/notifications/notifications";

export const store = configureStore({
    reducer: {
        game21: game21Reducer,
        playerInfo: playerInfoReducer,
        lobby: lobbyReducer,
        player: playerReducer,
        money: moneyReducer,
        notifications: notificationsReducer
    }
});

export type RootState = ReturnType<typeof store.getState>