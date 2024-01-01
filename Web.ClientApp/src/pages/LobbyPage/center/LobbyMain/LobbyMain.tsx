import {Lobby} from "shared/pages/main-page/Center/Main/lobby/Lobby/Lobby";
import {RoomsHead} from "shared/pages/main-page/Center/Main/lobby/Lobby/RoomsHead/RoomsHead";
import {
    RoomsSearch
} from "shared/pages/main-page/Center/Main/lobby/Lobby/RoomsHead/RoomsSearch/RoomsSearch";
import {RoomsList} from "shared/pages/main-page/Center/Main/lobby/Lobby/RoomsList/RoomsList";
import {
    RoomItemHead
} from "shared/pages/main-page/Center/Main/lobby/Lobby/RoomsList/RoomItemHead/RoomItemHead";
import {
    RoomItem
} from "shared/pages/main-page/Center/Main/lobby/Lobby/RoomsList/RoomItem/RoomItem";
import {Main} from "shared/pages/main-page/Center/Main/Main";
import {v4} from "uuid";
import {useRoomsList} from "BL/hooks/lobby/rooms-list/use-rooms-list";
import {MoneySelectorModal} from "shared/pages/main-page/Center/Main/lobby/MoneySelectorModal/MoneySelectorModal";
import {
    MoneySelectorInput
} from "shared/pages/main-page/Center/Main/lobby/MoneySelectorModal/MoneySelectorInput/MoneySelectorInput";
import {
    MoneySelectorButtons
} from "shared/pages/main-page/Center/Main/lobby/MoneySelectorModal/MoneySelectorButtons/MoneySelectorButtons";
import {useSelectStartMoney} from "BL/hooks/lobby/select-start-money/use-select-start-money";
import {
    useReconnectToRoomModalSelector,
    useSelectStartMoneyModalSelector
} from "BL/slices/modals/use-modals-selector";
import {
    ReconnectToRoomModal
} from "shared/pages/main-page/Center/Main/lobby/ReconnectToRoomModal/ReconnectToRoomModal";
import {
    ReconnectToRoomHead
} from "shared/pages/main-page/Center/Main/lobby/ReconnectToRoomModal/ReconnectToRoomHead/ReconnectToRoomHead";
import {
    ReconnectToRoomButtons
} from "shared/pages/main-page/Center/Main/lobby/ReconnectToRoomModal/ReconnectToRoomButtons/ReconnectToRoomButtons";

export const LobbyMain = () => {
    const {rooms, joinToRoomHandler} = useRoomsList();
    const {selectStartMoney, handlers} = useSelectStartMoney();
    const selectStartMoneyModal = useSelectStartMoneyModalSelector();
    const reconnectToRoomModal = useReconnectToRoomModalSelector();

    return (
        <Main>
            <Lobby>
                <RoomsHead>
                    <RoomsSearch/>
                </RoomsHead>
                {selectStartMoneyModal
                    ? (<MoneySelectorModal>
                            <MoneySelectorInput selector={selectStartMoney} selectStartMoneyHandler={handlers.selectStartMoneyHandler}/>
                            <MoneySelectorButtons handlers={handlers}/>
                        </MoneySelectorModal>)
                    : null}
                {reconnectToRoomModal
                    ? (<ReconnectToRoomModal>
                        <ReconnectToRoomHead/>
                        <ReconnectToRoomButtons/>
                    </ReconnectToRoomModal>)
                    : null}
                <RoomsList>
                    <RoomItemHead/>
                    {rooms.length
                        ? rooms.map(room => (<RoomItem key={v4()} joinToRoomHandler={joinToRoomHandler} roomData={room}/>))
                        : null}
                </RoomsList>
                {/*<RoomsPagination>*/}
                {/*</RoomsPagination>*/}
            </Lobby>
        </Main>
    )
}