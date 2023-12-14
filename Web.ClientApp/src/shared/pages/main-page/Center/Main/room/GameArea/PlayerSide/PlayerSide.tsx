import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./PlayerSide.module.css";

export const PlayerSide = ({children}: PropsWithChildren) => {
    return (
        <div className={style.playerSide}>
            {children}
        </div>
    )
}