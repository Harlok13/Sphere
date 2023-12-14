import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./Lobby.module.css";

export const Lobby = ({children}: PropsWithChildren) => {
    return (
        <div className={style.lobbies}>
            {children}
        </div>
    )
}