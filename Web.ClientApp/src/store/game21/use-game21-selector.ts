import {useTypedSelector} from "hooks/use-typed-selector";

export const useInRoomSelector = () =>
    useTypedSelector(state => state.game21.inRoom);

export const useGame21PlayersSelector = () =>
    useTypedSelector(state => state.game21.players);

export const useRoomDataSelector = () =>
    useTypedSelector(state => state.game21.roomData);

export const useRoomBankSelector = () =>
    useTypedSelector(state => state.game21.bank);

export const useGameStartedSelector = () =>
    useTypedSelector(state => state.game21.gameStarted);

export const useGameHistorySelector = () =>
    useTypedSelector(state => state.game21.gameHistory);