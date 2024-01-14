import {useTypedSelector} from "hooks/use-typed-selector";

export const usePlayerSelector = () =>
    useTypedSelector(state => state.player.player);


export const useTimerSelector = () =>
    useTypedSelector(state => state.player.timer);

