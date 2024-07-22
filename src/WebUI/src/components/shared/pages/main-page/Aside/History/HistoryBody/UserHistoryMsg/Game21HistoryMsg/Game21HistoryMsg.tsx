import style from "./Game21HistoryMsg.module.css";

interface IGame21HistoryData {  // TODO: relocate to interfaces
    gameResult: string;  // enum  //TODO: make enum
    userName: string;
    opponentName: string;
    score: string;
}

const Game21HistoryMsg = (historyData: IGame21HistoryData) => {  // TODO: props and decompos
    return (
        <>
            <img src="/img/avatars/me.jpg" alt="avatar" className={style.image}/>
            <div className={style.body}>
                <h2 className={style.versusTitle}>{historyData.userName} <span
                    className={style.vs}>vs</span> {historyData.opponentName}</h2>
                <div className={style.info}>
                    <div className={style.state}>{historyData.gameResult}</div>
                    <div className={style.score}>{historyData.score}</div>
                </div>
            </div>
            <img className={style.image} src="/img/avatars/bot_avatar.jpg" alt="avatar"/>
        </>
    )
}

export {Game21HistoryMsg};