import {configureStore} from "@reduxjs/toolkit";
import game21Reducer from "./slices/game21.slice";
import userInfoReducer from "./slices/user-info.slice";
import lobbyReducer from "./slices/lobby.slice";
import playerReducer from "./slices/player.slice";

export default configureStore({
    reducer: {
        game21: game21Reducer,
        userInfo: userInfoReducer,
        lobby: lobbyReducer,
        player: playerReducer,
    }
});