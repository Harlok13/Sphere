import {useDispatch} from "react-redux";
import {useEffect} from "react";
import {setSelectStartMoneyModal} from "store/modals/modals.slice";

export const useLobbyPage = () => {
    const dispatch = useDispatch();

    useEffect(() => {
        return () => {
            dispatch(setSelectStartMoneyModal());
        }
    }, []);
}