import {Lobby} from "../../../../../shared/pages/main-page/Center/Main/lobby/Lobby/Lobby";
import {RoomsHead} from "../../../../../shared/pages/main-page/Center/Main/lobby/Lobby/RoomsHead/RoomsHead";
import {
    RoomsSearch
} from "../../../../../shared/pages/main-page/Center/Main/lobby/Lobby/RoomsHead/RoomsSearch/RoomsSearch";
import {RoomsList} from "../../../../../shared/pages/main-page/Center/Main/lobby/Lobby/RoomsList/RoomsList";
import {
    RoomItemHead
} from "../../../../../shared/pages/main-page/Center/Main/lobby/Lobby/RoomsList/RoomItemHead/RoomItemHead";
import {
    RoomItem
} from "../../../../../shared/pages/main-page/Center/Main/lobby/Lobby/RoomsList/RoomItem/RoomItem";
import {Main} from "../../../../../shared/pages/main-page/Center/Main/Main";
import {v4} from "uuid";
import {useRoomsList} from "../../../../../BL/hooks/lobby/rooms-list/use-rooms-list";

export const LobbyMain = () => {
    const {rooms, joinToRoomHandler} = useRoomsList();
    // debugger
    return (
        <Main>
            <Lobby>
                <RoomsHead>
                    <RoomsSearch/>
                </RoomsHead>
                <RoomsList>
                    <RoomItemHead/>
                    {rooms.length
                        // ? rooms.map(l => (<RoomItem key={v4()} guid={l.guid} joinToRoomHandler={joinToRoomHandler} imgUrl={l.imgUrl} roomName={l.roomName} roomSize={l.roomSize} startBid={l.startBid} minBid={l.minBid} maxBid={l.maxBid} status={l.status} playersInRoom={l.playersInRoom}/>))
                        ? rooms.map(l => (<RoomItem key={v4()} joinToRoomHandler={joinToRoomHandler} props={l}/>))
                        : null}
                </RoomsList>
                {/*<RoomsPagination>*/}
                {/*</RoomsPagination>*/}
            </Lobby>
        </Main>
    )
}