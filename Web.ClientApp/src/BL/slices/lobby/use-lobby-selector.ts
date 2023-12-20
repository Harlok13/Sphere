import {useTypedSelector} from "../../use-typed-selector";


export const useRoomsSelector = () => {
    return useTypedSelector(state => state.lobby.rooms);
}

export const useNewRoomConfigSelector = () => {
    return useTypedSelector(state => state.lobby.newRoomConfig);
}

