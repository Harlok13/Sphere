import {PropsWithChildren} from "react";
import style from "./AboutRoomList.module.css";

const AboutRoomList = ({children}: PropsWithChildren) => {
    return (
        <ul className={style.list}>
            {children}
        </ul>
    )
}

export {AboutRoomList};