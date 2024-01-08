import {PropsWithChildren} from "react";
import style from "./HistoryBody.module.css";

const HistoryBody = ({children}: PropsWithChildren<{}>) => {
    return (
        <div className={style.body}>
            {children}
        </div>
    )
}

export {HistoryBody};