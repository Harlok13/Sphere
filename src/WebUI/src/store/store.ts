import {configureStore} from "@reduxjs/toolkit";
import game21Reducer from "store/game21/game21.slice";
import playerInfoReducer from "store/player-info/player-info.slice";
import lobbyReducer from "store/lobby/lobby.slice";
import playerReducer from "store/player/player.slice";
import moneyReducer from "store/money/money.slice";
import notificationsReducer from "store/notifications/notifications.slice";
import modalsReducer from "store/modals/modals.slice";


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