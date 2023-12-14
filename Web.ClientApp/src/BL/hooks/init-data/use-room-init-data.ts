import {useDispatch, useSelector} from "react-redux";
import {setInRoom, setNewPlayer} from "../../slices/game21.slice";
import {useEffect} from "react";

export const useRoomInitData = () => {
    const dispatch = useDispatch();
    // @ts-ignore
    const userInfo = useSelector(state => state.userInfo);
    const newPlayer = {
        playerId: userInfo.userId,
        playerName: userInfo.userName,
        readiness: false,
        isLeader: false
    }

    useEffect(() => {  // TODO: set room chat visible
        dispatch(setInRoom(true));
    }, []);
}