import {useRoomsList} from "hooks/lobby/rooms-list/use-rooms-list";
import {useSelectStartMoney} from "hooks/lobby/select-start-money/use-select-start-money";
import {useReconnectToRoomModalSelector, useSelectStartMoneyModalSelector} from "store/modals/use-modals-selector";
import {Main} from "components/shared/pages/main-page/Center/Main/Main";
import {Lobby} from "components/shared/pages/main-page/Center/Main/lobby/Lobby/Lobby";
import {RoomsHead} from "components/shared/pages/main-page/Center/Main/lobby/Lobby/RoomsHead/RoomsHead";
import {RoomsSearch} from "components/shared/pages/main-page/Center/Main/lobby/Lobby/RoomsHead/RoomsSearch/RoomsSearch";
import {
    MoneySelectorModal
} from "components/shared/pages/main-page/Center/Main/lobby/MoneySelectorModal/MoneySelectorModal";
import {
    MoneySelectorInput
} from "components/shared/pages/main-page/Center/Main/lobby/MoneySelectorModal/MoneySelectorInput/MoneySelectorInput";
import {
    MoneySelectorButtons
} from "components/shared/pages/main-page/Center/Main/lobby/MoneySelectorModal/MoneySelectorButtons/MoneySelectorButtons";
import {
    ReconnectToRoomModal
} from "components/shared/pages/main-page/Center/Main/lobby/ReconnectToRoomModal/ReconnectToRoomModal";
import {
    ReconnectToRoomHead
} from "components/shared/pages/main-page/Center/Main/lobby/ReconnectToRoomModal/ReconnectToRoomHead/ReconnectToRoomHead";
import {
    ReconnectToRoomButtons
} from "components/shared/pages/main-page/Center/Main/lobby/ReconnectToRoomModal/ReconnectToRoomButtons/ReconnectToRoomButtons";
import {RoomsList} from "components/shared/pages/main-page/Center/Main/lobby/Lobby/RoomsList/RoomsList";
import {
    RoomItemHead
} from "components/shared/pages/main-page/Center/Main/lobby/Lobby/RoomsList/RoomItemHead/RoomItemHead";
import {RoomItem} from "components/shared/pages/main-page/Center/Main/lobby/Lobby/RoomsList/RoomItem/RoomItem";
import {v4} from "uuid";


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