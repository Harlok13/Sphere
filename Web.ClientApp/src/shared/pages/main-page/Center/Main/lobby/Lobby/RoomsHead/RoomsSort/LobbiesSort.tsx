import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./LobbiesSort.module.css";

export const LobbiesSort = ({children}: PropsWithChildren) => {
    return (
        <div className={style.sortContainer}>
            {children}
        </div>
    )
}