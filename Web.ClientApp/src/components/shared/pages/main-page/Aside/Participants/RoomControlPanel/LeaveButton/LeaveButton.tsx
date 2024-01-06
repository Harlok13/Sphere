import style from "../RoomControlPanel.module.css";
import React, {FC} from "react";

interface RemoveFromRoomProps {
    removeFromRoomHandler: (e: React.MouseEvent<HTMLButtonElement>) => Promise<void>
}

export const LeaveButton: FC<RemoveFromRoomProps> = ({removeFromRoomHandler}) => {

    return (
        <button onClick={removeFromRoomHandler} className={`${style.leave} ${style.button}`}>
            Leave
            <span className={`${style.iconBottom} material-icons-outlined`}>
                logout
            </span>
        </button>
    )
}