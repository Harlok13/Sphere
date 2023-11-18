import {PropsWithChildren, useEffect, useRef} from "react";
// @ts-ignore
import style from "./style.module.css";

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

