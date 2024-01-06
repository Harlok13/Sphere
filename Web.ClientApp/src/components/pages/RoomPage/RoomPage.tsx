import {useRoomInitData} from "hooks/init-data/use-room-init-data";
import {useRemoveFromRoomHub} from "hooks/hub-connection/server-methods/server-methods";
import {useNavigate} from "react-router-dom";
import {usePlayerSelector} from "store/player/use-player-selector";
import {useDispatch} from "react-redux";
import React, {useEffect} from "react";
import {NavigateEnum} from "shared/constants/navigate.enum";
import {resetModalsState, setReconnectToRoomModal} from "store/modals/modals.slice";
import {resetGame21State} from "store/game21/game21.slice";
import {BeforeUnload} from "components/pages/RoomPage/BeforeUnload/BeforeUnload";
import {ContentContainer} from "components/shared/pages/main-page/ContentContainer/ContentContainer";
import {GlobalAside} from "components/layout/GlobalAside/GlobalAside";
import {RoomCenter} from "components/pages/RoomPage/RoomCenter/RoomCenter";
import {GlobalRightSide} from "components/layout/GlobalRightSide/GlobalRightSide";


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
