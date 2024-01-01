import {configureStore} from "@reduxjs/toolkit";
import game21Reducer from "BL/slices/game21/game21.slice";
import playerInfoReducer from "BL/slices/player-info/player-info.slice";
import lobbyReducer from "BL/slices/lobby/lobby.slice";
import playerReducer from "BL/slices/player/player.slice";
import moneyReducer from "BL/slices/money/money.slice";
import notificationsReducer from "BL/slices/notifications/notifications";
import modalsReducer from "BL/slices/modals/modals.slice";

export const store = configureStore({
    reducer: {
        game21: game21Reducer,
        playerInfo: playerInfoReducer,
        lobby: lobbyReducer,
        player: playerReducer,
        money: moneyReducer,
        notifications: notificationsReducer,
        modals: modalsReducer,
    }
});

export type RootState = ReturnType<typeof store.getState>