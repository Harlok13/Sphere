import {PropsWithChildren} from "react";
import style from "./RoomControlPanel.module.css";

export const RoomControlPanel = ({children}: PropsWithChildren) => {
    return (
        <div className={style.roomControlPanel}>
            {children}
        </div>
    )
}