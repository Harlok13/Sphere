import {PropsWithChildren} from "react";
import style from "./RoomsList.module.css";

export const RoomsList = ({children}: PropsWithChildren) => {
    return (
        <ul className={style.list}>
            {children}
        </ul>
    )
}