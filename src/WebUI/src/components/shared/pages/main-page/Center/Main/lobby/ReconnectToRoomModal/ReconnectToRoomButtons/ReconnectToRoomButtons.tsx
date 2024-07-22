import {ReconnectToRoomHandlers} from "hooks/lobby/reconnect-to-room/use-reconnect-to-room";
import {FC} from "react";

export const ReconnectToRoomButtons: FC<{
    handlers: ReconnectToRoomHandlers;
}> = ({handlers}) => {
    const {confirmReconnectingToRoomHandler, cancelReconnectingToRoomHandler} = handlers;

    return (
        <div>
            <button onClick={(e) => confirmReconnectingToRoomHandler(e)}>Confirm</button>
            <button onClick={cancelReconnectingToRoomHandler}>Cancel</button>
        </div>
    )
}