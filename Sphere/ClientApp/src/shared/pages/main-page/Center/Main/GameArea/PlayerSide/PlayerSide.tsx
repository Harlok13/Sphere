import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./style.module.css";

export const PlayerSide = ({children}: PropsWithChildren) => {
    return (
        <div className={style.playerSide}>
            {children}
        </div>
    )
}