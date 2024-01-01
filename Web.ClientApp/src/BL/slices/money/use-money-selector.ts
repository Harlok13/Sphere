import {useTypedSelector} from "../../use-typed-selector";

export const useSelectStartMoneySelector = () =>
    useTypedSelector(state => state.money.selectStartMoney);

export const useSelectStartMoneyTypeSelector = () =>
    useTypedSelector(state => state.money.type)