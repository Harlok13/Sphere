import {PropsWithChildren, useRef} from "react";
import style from "./GameHistory.module.css";

const GameHistory = ({children}: PropsWithChildren) => {
    const gameHistoryRef = useRef(null);

    // useEffect(() => {
    //     scrollToBottom();
    // }, [props.gameHistoryMessages]);

    const scrollToBottom = () => {
        if (gameHistoryRef.current){
            gameHistoryRef.current.scrollTop = gameHistoryRef.current.scrollHeight;
        }
    }

    return (
        <div className={style.gameHistory} ref={gameHistoryRef}>
            {children}
        </div>
    )
}

export {GameHistory};

