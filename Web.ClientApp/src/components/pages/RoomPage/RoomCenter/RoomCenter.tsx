import {Center} from "../../../../shared/pages/main-page/Center/Center";
import {RoomBottom} from "./RoomBottom/RoomBottom";
import {RoomMain} from "./RoomMain/RoomMain";
import {GlobalHead} from "../../../layout/GlobalHead/GlobalHead";

export const RoomCenter = () => {
    return (
        <Center>
            <GlobalHead/>
            <RoomMain/>
            <RoomBottom/>
        </Center>
    )
}