import {useTypedSelector} from "hooks/use-typed-selector";


export const useSelectStartMoneyModalSelector = () =>
    useTypedSelector(state => state.modals.selectStartMoneyModal);

export const useReconnectToRoomModalSelector = () =>
    useTypedSelector(state => state.modals.reconnectToRoomModal);

export const usePlayerActionModalSelector = () =>
    useTypedSelector(state => state.modals.playerActionModal);