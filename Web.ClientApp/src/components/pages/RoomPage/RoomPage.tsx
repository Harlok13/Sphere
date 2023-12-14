import {ContentContainer} from "../../../shared/pages/main-page/ContentContainer/ContentContainer";
import {RoomCenter} from "./RoomCenter/RoomCenter";
import {GlobalAside} from "../../layout/GlobalAside/GlobalAside";
import {GlobalRightSide} from "../../layout/GlobalRightSide/GlobalRightSide";
import {useEffect} from "react";
import {signalRConnection} from "../../../App";
import {useHubMethod} from "react-use-signalr";
import {useRoomInitData} from "../../../BL/hooks/init-data/use-room-init-data";
import {useRemoveFromRoomHub} from "../../../BL/hooks/hub-connection/server-methods/server-methods";
import {useDispatch, useSelector} from "react-redux";
import {clearPlayers, setInRoom} from "../../../BL/slices/game21.slice";
import {BeforeUnload} from "./BeforeUnload/BeforeUnload";
import {useNavigate} from "react-router-dom";
import {NavigateEnum} from "../../../constants/navigate.enum";


export const RoomPage = () => {
    useRoomInitData();

    const navigate = useNavigate();

    const removeFromRoom = useRemoveFromRoomHub();

    const dispatch = useDispatch();

    // @ts-ignore
    const game21 = useSelector(state => state.game21);
    // @ts-ignore
    const player = useSelector(state => state.player);

    useEffect(() => {
        // setInterval(() => {
        //
        // }, 3000);
        // if (game21.player.roomGuid === ""){
        console.log(player);
        console.log(game21)
        if (player.roomGuid === ""){
            navigate(NavigateEnum.Lobby);
        }


        return () => {
            console.log("removed from room");
            const query = async () => {
                await removeFromRoom.invoke(player.roomGuid, player.id);
            }
            // dispatch(removePlayerData());  // TODO finish
            dispatch(setInRoom(false));
            dispatch(clearPlayers())
            query();
        }
    }, []);

    return (
        <BeforeUnload>
            <ContentContainer>
                <GlobalAside/>
                <RoomCenter/>
                <GlobalRightSide/>
            </ContentContainer>
        </BeforeUnload>
    )
}
