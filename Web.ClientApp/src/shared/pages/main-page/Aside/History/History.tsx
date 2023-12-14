import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./History.module.css";

const History = ({children}: PropsWithChildren<{}>) => {
    return (
        <div className={style.history}>
            {children}
        </div>
    )
}

export {History};