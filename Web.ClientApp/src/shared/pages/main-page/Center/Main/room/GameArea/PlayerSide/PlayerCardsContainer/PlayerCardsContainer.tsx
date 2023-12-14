import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./PlayerCardsContainer.module.css";

export const PlayerCardsContainer = ({children}: PropsWithChildren) => {
    return (
        <div className={style.playerCardsContainer}>
            {children}
        </div>
    )
}