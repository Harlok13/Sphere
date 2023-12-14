import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./RoomsPagination.module.css";

export const RoomsPagination = ({children}: PropsWithChildren) => {
    return (
        <div className={style.pagination}>
            {children}
        </div>
    )
}