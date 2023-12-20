import {useDispatch} from "react-redux";
import {useEffect} from "react";
import {setImgUrl} from "../../slices/lobby/lobby.slice";
import {usePlayerInfoSelector} from "../../slices/player-info/use-player-info-selector";

export const useLobbyInitData = () => {
    const dispatch = useDispatch();
    const playerInfo = usePlayerInfoSelector();

    useEffect(() => {
        // remove <Participants/> component

        // initialize data for creating a room
        dispatch(setImgUrl(playerInfo.avatarUrl));
    }, []);
}