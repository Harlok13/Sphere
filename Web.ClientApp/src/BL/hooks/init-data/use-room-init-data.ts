import {useDispatch} from "react-redux";
import {setInRoom} from "../../slices/game21/game21.slice";
import {useEffect} from "react";

export const useRoomInitData = () => {
    const dispatch = useDispatch();

    useEffect(() => {  // TODO: set room chat visible
        dispatch(setInRoom(true));
    }, []);
}