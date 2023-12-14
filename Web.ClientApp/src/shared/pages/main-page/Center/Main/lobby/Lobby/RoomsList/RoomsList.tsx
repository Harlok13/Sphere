import {PropsWithChildren, useEffect, useRef} from "react";
// @ts-ignore
import style from "./RoomsList.module.css";

export const RoomsList = ({children}: PropsWithChildren) => {
    return (
        <ul className={style.list}>
            {children}
        </ul>
    )
}