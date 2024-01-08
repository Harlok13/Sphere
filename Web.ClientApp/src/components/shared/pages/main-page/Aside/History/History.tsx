import {PropsWithChildren} from "react";
import style from "./History.module.css";

export const History = ({children}: PropsWithChildren<{}>) => {
    return (
        <div className={style.history}>
            {children}
        </div>
    )
}