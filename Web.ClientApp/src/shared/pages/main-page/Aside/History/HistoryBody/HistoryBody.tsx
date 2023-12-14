import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./HistoryBody.module.css";

const HistoryBody = ({children}: PropsWithChildren<{}>) => {
    return (
        <div className={style.body}>
            {children}
        </div>
    )
}

export {HistoryBody};