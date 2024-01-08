import {PropsWithChildren} from "react";
import style from "./RoomChat.module.css";

export const RoomChat = ({children}: PropsWithChildren) => {
    return (
        <div className={style.chat}>
            {children}
        </div>
    )
}