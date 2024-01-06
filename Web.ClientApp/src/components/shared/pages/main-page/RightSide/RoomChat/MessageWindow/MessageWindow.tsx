import {PropsWithChildren} from "react";
import style from "./MessageWindow.module.css";

export const MessageWindow = ({children}: PropsWithChildren) => {
    return (
        <div className={style.messageWindow}>
            {children}
        </div>
    )
}