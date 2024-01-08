import {useDispatch} from "react-redux";
import {useEffect} from "react";
import {setInRoom} from "store/game21/game21.slice";

export const useRoomInitData = () => {
    const dispatch = useDispatch();

    useEffect(() => {  // TODO: set room chat visible
        dispatch(setInRoom(true));
    }, []);
}