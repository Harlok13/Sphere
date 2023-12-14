// @ts-ignore
import style from "./style.module.css";
import HistoryMessage from "./GameHistory/HistoryMessage/HistoryMessage";
import {useEffect, useRef} from "react";
import {v4} from "uuid";
import {NotificationPanel} from "./NotificationPanel/NotificationPanel";

const Bottom = ({props}) => {
    const gameHistoryRef = useRef(null);

    useEffect(() => {
        scrollToBottom();
    }, [props.gameHistoryMessages]);

    const scrollToBottom = () => {
        if (gameHistoryRef.current){
            gameHistoryRef.current.scrollTop = gameHistoryRef.current.scrollHeight;
        }
    }


    return (
        <div className={style.bottom}>
            <div className={style.controlPanel}>
                {props.game
                    ? (
                        <>
                            <button className={style.button} onClick={props.getCardHandler} ref={props.getCardRef}>Get card</button>
                            <button className={style.button} onClick={props.passHandler} ref={props.passRef}>Pass</button>
                        </>
                    )
                    : (<button className={style.button} onClick={props.startGameHandler} ref={props.startGameRef}>Start Game</button>)
                }
            </div>
            <div className={`${style.gameHistory} ${style.resize}`} ref={gameHistoryRef}>
                <div className={style.body}>
                    <div className={style.title}>Game History</div>
                    <div className={style.info}>
                        <ul className={style.list}>
                            {/*<li className={style.item}><span className={style.time}>21:32</span><span className={style.gameState}>Start game</span></li>*/}
                            {/*<li className={style.item}><span className={style.time}>21:32</span><span className={style.userColor}>Harlok</span> get card: <span className={style.value}>10 peaks</span>. Score: <span className={style.value}>10</span></li>*/}
                            {/*<li className={style.item}><span className={style.time}>21:33</span><span className={style.opponentColor}>Bot</span> get card: <span className={style.value}>7 peaks</span>. Score: <span className={style.value}>7</span></li>*/}
                            {props.gameHistoryMessages.length
                                ? props.gameHistoryMessages.map(msgData => (<HistoryMessage key={v4()} msgData={msgData}/>))
                                : null
                            }
                        </ul>
                    </div>
                </div>
            </div>
            <div className={style.roomInfo}>
                <div className={style.body}>
                    <div className={style.title}>Room info</div>
                    <div className={style.info}>
                        <ul className={style.list}>
                            <li className={style.item}>Size room: <span className={style.value}>2/2</span></li>
                            <li className={style.item}>Start bid: <span className={style.value}>100$</span>;</li>
                            <li className={style.item}>Bid: <span className={style.value}>5/20$</span>;</li>
                        </ul>
                    </div>
                </div>
            </div>
            <NotificationPanel props={props}/>
        </div>
    )
}

export default Bottom;
