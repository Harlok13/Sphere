import {useRoomsList} from "hooks/lobby/rooms-list/use-rooms-list";
import {useSelectStartMoney} from "hooks/lobby/select-start-money/use-select-start-money";
import {useReconnectToRoomModalSelector, useSelectStartMoneyModalSelector} from "store/modals/use-modals-selector";
import {Main} from "components/shared/pages/main-page/Center/Main/Main";
import {Lobby} from "components/shared/pages/main-page/Center/Main/lobby/Lobby/Lobby";
import {RoomsHead} from "components/shared/pages/main-page/Center/Main/lobby/Lobby/RoomsHead/RoomsHead";
import {RoomsSearch} from "components/shared/pages/main-page/Center/Main/lobby/Lobby/RoomsHead/RoomsSearch/RoomsSearch";
import {
    MoneySelectorInput
} from "components/shared/pages/main-page/Center/Main/lobby/MoneySelectorModal/MoneySelectorInput/MoneySelectorInput";
import {
    MoneySelectorButtons
} from "components/shared/pages/main-page/Center/Main/lobby/MoneySelectorModal/MoneySelectorButtons/MoneySelectorButtons";
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
import {Modal} from "components/shared/components/Modal/Modal";
import {useDispatch} from "react-redux";
import {setReconnectToRoomModal, setSelectStartMoneyModal} from "store/modals/modals.slice";
import {ReconnectToRoomHandlers, useReconnectToRoom} from "hooks/lobby/reconnect-to-room/use-reconnect-to-room";


export const LobbyMain = () => {
    const {rooms, joinToRoomHandler} = useRoomsList();
    const {selectStartMoney, handlers} = useSelectStartMoney();
    const selectStartMoneyModal = useSelectStartMoneyModalSelector();
    const reconnectToRoomModal = useReconnectToRoomModalSelector();

    const dispatch = useDispatch();
    const closeSelectStartMoneyModalHandler = () => {
        dispatch(setSelectStartMoneyModal(false))
    }

    const closeReconnectToRoomModal = () => {
        dispatch(setReconnectToRoomModal(false));
    }

    const reconnectToRoomHandlers: ReconnectToRoomHandlers = useReconnectToRoom();

    return (
        <Main>
            <Lobby>
                <RoomsHead>
                    <RoomsSearch/>
                </RoomsHead>
                <Modal
                    isOpen={selectStartMoneyModal}
                    onClose={closeSelectStartMoneyModalHandler}
                    timeout={350}
                >
                    <MoneySelectorInput
                        selector={selectStartMoney}
                        selectStartMoneyHandler={handlers.selectStartMoneyHandler}
                    />
                    <MoneySelectorButtons
                        handlers={handlers}
                    />
                </Modal>
                <Modal
                    isOpen={reconnectToRoomModal}
                    onClose={closeReconnectToRoomModal}
                    timeout={350}
                >
                    <ReconnectToRoomHead/>
                    <ReconnectToRoomButtons
                        handlers={reconnectToRoomHandlers}
                    />
                </Modal>
                <RoomsList>
                    <RoomItemHead/>
                    {rooms.length > 0 && rooms.map(room => (
                        <RoomItem
                            key={v4()}
                            joinToRoomHandler={joinToRoomHandler}
                            roomData={room}
                        />))}
                </RoomsList>
                {/*<RoomsPagination>*/}
                {/*</RoomsPagination>*/}
            </Lobby>
        </Main>
    )
}