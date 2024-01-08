import style from "./GameHistoryMsg.module.css";
import {GameHistoryMessage} from "shared/contracts/responses/added-game-history-message-response";
import {FC} from "react";

const GameHistoryMsg: FC<{
    msgData: GameHistoryMessage;
}> = ({msgData}) => {
    let content;

    switch (msgData.type){  // TODO: use enums
        case "GameState":
            content = (
                <li className={style.item}>
                    <span className={style.time}>{msgData.currentTime}</span> <span className={style.gameState}>{msgData.message}</span>
                </li>
            )
            break;

        case "Hit":
            content = (
                <li className={style.item}>
                    {/*<span className={style.time}>{msgData.currentTime}</span> <span className={msgData.playerNameColor === "user" ? `${style.userColor}` : `${style.opponentColor}`}>{msgData.playerName}</span> get card <span className={style.value}>{msgData.cardValue}</span>. Score: <span className={style.value}>{msgData.playerScoreValue}</span>*/}
                </li>
            )
            break;

        case "Stay":
            content = (
                <div></div>
            )
            break;
        default: content = null;

    }

    return content;
}

export {GameHistoryMsg};