import {PropsWithChildren} from "react";
import style from "./PlayerSide.module.css";

export const PlayerSide = ({children}: PropsWithChildren) => {
    return (
        <div className={style.playerSide}>
            {children}
        </div>
    )
}