import {PropsWithChildren} from "react";
import style from "./PlayerCardsContainer.module.css";

export const PlayerCardsContainer = ({children}: PropsWithChildren) => {
    return (
        <div className={style.playerCardsContainer}>
            {children}
        </div>
    )
}