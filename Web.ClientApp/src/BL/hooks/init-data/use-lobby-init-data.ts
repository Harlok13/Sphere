import {useDispatch, useSelector} from "react-redux";
import {useEffect} from "react";
import {setImgUrl} from "../../slices/lobby.slice";
import {v4} from "uuid";

export const useLobbyInitData = () => {
    const dispatch = useDispatch();
    // @ts-ignore
    const userInfo = useSelector(state => state.userInfo);

    useEffect(() => {
        // remove <Participants/> component

        // initialize data for creating a room
        // dispatch(setRoomGuid(v4()));
        dispatch(setImgUrl(userInfo.avatar));
    }, []);
}