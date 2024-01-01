import {useEffect} from "react";
import {useDispatch} from "react-redux";
import InitDataService from "../../../services/api/init-data.service";
import {initPlayerInfo} from "../../slices/player-info/player-info.slice";
import {initRooms} from "../../slices/lobby/lobby.slice";

export const useGlobalInitData = () => {
    const dispatch = useDispatch();

    useEffect(() => {
        const setInitData = async() => {
            const data = await InitDataService.getInitData();
            dispatch(initPlayerInfo(data.playerInfo));
            if (!data.rooms) data.rooms = [];
            dispatch(initRooms(data.rooms));
        }

        setInitData();
    }, [])
}