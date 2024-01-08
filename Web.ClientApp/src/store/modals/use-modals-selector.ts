import {useTypedSelector} from "hooks/use-typed-selector";


export const useSelectStartMoneyModalSelector = () =>
    useTypedSelector(state => state.modals.selectStartMoneyModal);

export const useReconnectToRoomModalSelector = () =>
    useTypedSelector(state => state.modals.reconnectToRoomModal);

export const useParticipantActionsModalSelector = () =>
    useTypedSelector(state => state.modals.participantActionsModal);