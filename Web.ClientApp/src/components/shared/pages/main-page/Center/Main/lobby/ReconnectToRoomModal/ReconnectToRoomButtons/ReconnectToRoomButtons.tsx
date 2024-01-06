import {useReconnectToRoom} from "hooks/lobby/reconnect-to-room/use-reconnect-to-room";

export const ReconnectToRoomButtons = () => {
    const {confirmReconnectingToRoom, cancelReconnectingToRoomHandler} = useReconnectToRoom();

    return (
        <div>
            <button onClick={(e) => confirmReconnectingToRoom(e)}>Confirm</button>
            <button onClick={cancelReconnectingToRoomHandler}>Cancel</button>
        </div>
    )
}