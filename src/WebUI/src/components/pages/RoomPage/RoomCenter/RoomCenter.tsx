import {Center} from "components/shared/pages/main-page/Center/Center";
import {RoomMain} from "components/pages/RoomPage/RoomCenter/RoomMain/RoomMain";
import {RoomBottom} from "components/pages/RoomPage/RoomCenter/RoomBottom/RoomBottom";

export const RoomCenter = () => {
    return (
        <Center>
            {/*<GlobalHead/>*/}
            <RoomMain/>
            <RoomBottom/>
        </Center>
    )
}