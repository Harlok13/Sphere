import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./GameArea.module.css";

export const GameArea = ({children}: PropsWithChildren) => {
    return (
        <div className={style.gameArea}>
            {children}
        </div>
    )
}