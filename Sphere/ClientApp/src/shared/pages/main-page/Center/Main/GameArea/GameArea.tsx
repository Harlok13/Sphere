import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./style.module.css";

export const GameArea = ({children}: PropsWithChildren) => {
    return (
        <div className={style.gameArea}>
            {children}
        </div>
    )
}