import {useTypedSelector} from "../../use-typed-selector";

export const useSelectStartMoneySelector = () => {
    return useTypedSelector(state => state.money.selectStartMoney);
}

export const useShowModalSelector = () => {
    return useTypedSelector(state => state.money.showModal);
}

export const useSelectStartMoneyTypeSelector = () =>
    useTypedSelector(state => state.money.type)