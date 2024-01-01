import {ContentContainer} from "shared/pages/main-page/ContentContainer/ContentContainer";
import {RoomCenter} from "pages/RoomPage/RoomCenter/RoomCenter";
import {GlobalAside} from "components/layout/GlobalAside/GlobalAside";
import {GlobalRightSide} from "components/layout/GlobalRightSide/GlobalRightSide";
import {useEffect} from "react";
import {useRoomInitData} from "BL/hooks/init-data/use-room-init-data";
import {useRemoveFromRoomHub} from "BL/hooks/hub-connection/server-methods/server-methods";
import {useDispatch} from "react-redux";
import {resetGame21State} from "BL/slices/game21/game21.slice";
import {BeforeUnload} from "pages/RoomPage/BeforeUnload/BeforeUnload";
import {useNavigate} from "react-router-dom";
import {NavigateEnum} from "constants/navigate.enum";
import {usePlayerSelector} from "BL/slices/player/use-player-selector";
import {IRemoveFromRoomRequest} from "contracts/requests/remove-from-room-request";
import {resetModalsState, setReconnectToRoomModal} from "BL/slices/modals/modals.slice";


export const RoomPage = () => {
    useRoomInitData();

    const navigate = useNavigate();

    const removeFromRoom = useRemoveFromRoomHub();

    const dispatch = useDispatch();

    const player = usePlayerSelector();

    useEffect(() => {
        if (player.roomId === ""){  // TODO: finish validation
            navigate(NavigateEnum.Lobby);
        }
        dispatch(setReconnectToRoomModal(false))

        return () => {
            console.log("removed from room");
            // const query = async () => {
            //     const removeFromRoomRequest: IRemoveFromRoomRequest = {
            //         roomId: player.roomId,
            //         playerId: player.id
            //     }
            //     // await removeFromRoom
            //     //     .invoke(removeFromRoomRequest);
            // }
            // dispatch(setInRoom(false));
            // dispatch(clearPlayersList());
            dispatch(resetGame21State());
            dispatch(resetModalsState())
            // query();
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
