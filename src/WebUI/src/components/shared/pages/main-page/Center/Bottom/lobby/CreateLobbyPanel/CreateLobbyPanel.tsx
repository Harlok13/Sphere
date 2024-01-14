import {PropsWithChildren} from "react";
import style from "./CreateLobby.module.css";

export const CreateLobbyPanel = ({children}: PropsWithChildren) => {
    return (
        <div className={style.container}>
            {children}
        </div>
    )
}