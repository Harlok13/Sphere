import {PropsWithChildren} from "react";
import style from "./GameHistoryList.module.css";

const GameHistoryList = ({children}: PropsWithChildren) => {
    return (
        <ul className={style.list}>
            {children}
        </ul>
    )
}

export {GameHistoryList};