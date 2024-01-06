import style from "./UserHistoryMsg.module.css";
import React from "react";

export type GameResultData = {  // TODO: relocate to types
    // result: "Win" | "Lose" | "Draw";
    result: string;
}

type UserHistoryProps = {
    children: React.ReactNode;
    // gameResult: GameResultData;
    gameResult: string;
}

export const UserHistoryMsg = ({children, gameResult}: UserHistoryProps) => {
    return (
        <div className={`${style.item} ${gameResult === "Win" ? style.item_win : gameResult === "Lose" ? style.item_lose : style.item_draw}`}>
            {children}
        </div>
    )
}