import {useEffect} from "react";
import {useDispatch, useSelector} from "react-redux";
import InitDataService from "../../../services/api/init-data.service";
import {setUser} from "../../slices/user-info.slice";
import {initRooms} from "../../slices/lobby.slice";

export const useGlobalInitData = () => {
    // @ts-ignore
    const dispatch = useDispatch();

    useEffect(() => {
        const setInitData = async() => {
            const data = await InitDataService.getInitData();
            console.log(data);

            dispatch(setUser(data));
            if (!data.roomResponse) data.roomResponse = [];
            dispatch(initRooms(data.roomResponse));

            // if (data.room){
            //     dispatch()
            // }
        }

        setInitData();
    }, [])
}