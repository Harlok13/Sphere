import {useTypedSelector} from "../../use-typed-selector";

export const usePlayerInfoSelector = () => {
    return useTypedSelector(state => state.playerInfo.playerInfo);
}

export const usePlayerHistorySelector = () => {
    return useTypedSelector(state => state.playerInfo.playerHistory);
}