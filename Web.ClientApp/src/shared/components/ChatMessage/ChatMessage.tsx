import style from "./ChatMessage.module.css";
import {FC} from "react";
// TODO: example {currentTime: "21:35", userName: "Harlok", text: "hello world"}

export type Message = {
    currentTime: string;  // TODO: fix type
    playerName: string;
    msgText: string;
}

interface IChatMessageProps {
    msgData: Message
}

export const ChatMessage: FC<IChatMessageProps> = ({msgData}) => {
    return (
        <div className={style.message}>
            <div className={style.leftSide}>
                <img className={style.avatar} src="/img/avatars/ava.jpg" alt="ava"/>
                <div className={style.time}>{msgData.currentTime}</div>
            </div>
            <div className={style.body}>
                <div className={style.messageHead}>
                    <div className={style.username}>{msgData.playerName}</div>
                    <span className={`${style.menu} material-icons-outlined`}>
                        more_vert
                    </span>
                </div>
                <div className={style.text}>{msgData.msgText}</div>
            </div>
        </div>
    )
}
