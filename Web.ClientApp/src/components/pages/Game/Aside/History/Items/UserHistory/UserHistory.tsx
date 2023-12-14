// @ts-ignore
import style from "../../style.module.css";

const UserHistory = ({historyData}) => {
    return (
        <div className={`${style.item} ${historyData.gameResult === "Win" ? style.item_win : historyData.gameResult === "Lose" ? style.item_lose : style.item_draw}`}>
            <img src="/img/avatars/me.jpg" alt="avatar" className={style.leftImage}/>
            <div className={style.body}>
                <h2 className={style.versusTitle}>{historyData.userName} <span className={style.vs}>vs</span> {historyData.opponentName}</h2>
                <div className={style.info}>
                    <div className={style.state}>{historyData.gameResult}</div>
                    <div className={style.score}>{historyData.score}</div>
                </div>
            </div>
            <img className={style.rightImage} src="/img/avatars/bot_avatar.jpg" alt="avatar"/>
        </div>
    )
}

export {UserHistory};