import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./GameHistoryList.module.css";

const GameHistoryList = ({children}: PropsWithChildren) => {
    return (
        <ul className={style.list}>
            {children}
        </ul>
    )
}

export {GameHistoryList};