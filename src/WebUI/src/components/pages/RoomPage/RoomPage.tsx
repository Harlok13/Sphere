import React from "react";
import {BeforeUnload} from "components/pages/RoomPage/BeforeUnload/BeforeUnload";
import {ContentContainer} from "components/shared/components/ContentContainer/ContentContainer";
import {GlobalAside} from "components/layout/GlobalAside/GlobalAside";
import {RoomCenter} from "components/pages/RoomPage/RoomCenter/RoomCenter";
import {GlobalRightSide} from "components/layout/GlobalRightSide/GlobalRightSide";
import {useRoomPage} from "hooks/room/room-page/use-room-page";


export const RoomPage = () => {
    useRoomPage();

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
