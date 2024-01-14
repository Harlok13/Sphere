import {PropsWithChildren} from "react";
import style from "./Lobby.module.css";

export const Lobby = ({children}: PropsWithChildren) => {
    return (
        <div className={style.lobbies}>
            {children}
        </div>
    )
}