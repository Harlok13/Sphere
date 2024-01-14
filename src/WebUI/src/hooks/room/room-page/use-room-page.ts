import {usePlayerSelector} from "store/player/use-player-selector";
import {useDispatch} from "react-redux";
import {useNavigate} from "react-router-dom";
import {useEffect} from "react";
import {NavigateEnum} from "shared/constants/navigate.enum";
import {resetModalsState, setReconnectToRoomModal} from "store/modals/modals.slice";
import {resetGame21State, setInRoom} from "store/game21/game21.slice";

export const useRoomPage = () => {
    const player = usePlayerSelector();

    const dispatch = useDispatch();
    const navigate = useNavigate();

    useEffect(() => {
        if (player.roomId === ""){
            navigate(NavigateEnum.Lobby);
        }
        else{
            dispatch(setInRoom(true));
            dispatch(setReconnectToRoomModal(false));
        }

        return () => {
            dispatch(resetGame21State());
            dispatch(resetModalsState());
        }
    }, []);
}