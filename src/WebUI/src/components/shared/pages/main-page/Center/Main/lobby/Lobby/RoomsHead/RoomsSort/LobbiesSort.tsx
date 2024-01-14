import {PropsWithChildren} from "react";
import style from "./LobbiesSort.module.css";

export const LobbiesSort = ({children}: PropsWithChildren) => {
    return (
        <div className={style.sortContainer}>
            {children}
        </div>
    )
}