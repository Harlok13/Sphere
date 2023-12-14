// TODO: example {historyMessageType: "getCard", playerNameColor: "opponent", playerName: "Bot", cardValue: "10 peaks", playerScoreValue: "17"}
// @ts-ignore
import style from "./style.module.css";

const HistoryMessage = ({msgData}) => {
    let content;


    switch (msgData.historyMessageType){  // TODO: use enums
        case "gameState":
            content = (
                <li className={style.item}>
                    <span className={style.time}>{msgData.currentTime}</span> <span className={style.gameState}>{msgData.gameState}</span>
                </li>
            )
            break;

        case "getCard":
            content = (
                <li className={style.item}>
                    <span className={style.time}>{msgData.currentTime}</span> <span className={msgData.playerNameColor === "user" ? `${style.userColor}` : `${style.opponentColor}`}>{msgData.playerName}</span> get card <span className={style.value}>{msgData.cardValue}</span>. Score: <span className={style.value}>{msgData.playerScoreValue}</span>
                </li>
            )
            break;

        case "pass":
            content = (
                <div></div>
            )
            break;
        default: content = (<div></div>);

    }

    return content;
}

export default HistoryMessage;

