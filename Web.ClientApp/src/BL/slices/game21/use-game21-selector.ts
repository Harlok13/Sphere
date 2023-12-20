import {useTypedSelector} from "../../use-typed-selector";

export const useInRoomSelector = () => {
    return useTypedSelector(state => state.game21.inRoom);
}

export const useGame21PlayersSelector = () => {
    return useTypedSelector(state => state.game21.players);
}

export const useRoomDataSelector = () => {
    return useTypedSelector(state => state.game21.roomData);
}

export const useRoomBankSelector = () => {
    return useTypedSelector(state => state.game21.bank);
}

export const useGameStartedSelector = () => {
    return useTypedSelector(state => state.game21.gameStarted);
}