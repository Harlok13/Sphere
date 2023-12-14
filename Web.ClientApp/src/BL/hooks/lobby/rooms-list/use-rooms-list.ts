import {useDispatch, useSelector} from "react-redux";
import {setInRoom} from "../../../slices/game21.slice";
import {useNavigate} from "react-router-dom";
import {NavigateEnum} from "../../../../constants/navigate.enum";
import {useJoinToRoomHub} from "../../hub-connection/server-methods/server-methods";

export const useRoomsList = () => {
    // @ts-ignore
    const lobby = useSelector(state => state.lobby);
    // @ts-ignore
    const player = useSelector(state => state.player);

    // @ts-ignore
    const userInfo = useSelector(state => state.userInfo);

    const joinToRoom = useJoinToRoomHub();

    const navigate = useNavigate();

    const dispatch = useDispatch();

    const joinToRoomHandler = async (e: React.MouseEvent<HTMLButtonElement>, guid: string) => {
        e.preventDefault();

        // await joinToRoom.invoke(guid, {
        //     playerId: userInfo.userId,
        //     playerName: userInfo.userName,
        //     avatar: userInfo.avatar,
        //     isLeader: false,
        //     readiness: false
        // });
        console.log(guid, userInfo.userId, userInfo.userName, "join to room data");
        await joinToRoom.invoke(guid, userInfo.userId, userInfo.userName);

        navigate(NavigateEnum.Room);  // TODO: check response. if not ok -> prohibit navigate
    }

    const rooms = lobby.rooms;

    return {rooms, joinToRoomHandler};
}
