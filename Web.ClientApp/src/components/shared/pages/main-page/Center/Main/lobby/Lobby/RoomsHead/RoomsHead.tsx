import {PropsWithChildren} from "react";
import style from "./RoomsHead.module.css";

export const RoomsHead = ({children}: PropsWithChildren) => {
    return (
        <div className={style.head}>
            <div className={style.title}>Rooms</div>
            {children}
        </div>
    )
}