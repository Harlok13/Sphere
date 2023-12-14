import style from "./ChatMessage.module.css";
// TODO: example {currentTime: "21:35", userName: "Harlok", text: "hello world"}

export const ChatMessage = ({msgData}) => {
    return (
        <div className={style.message}>
            <div className={style.leftSide}>
                <img className={style.avatar} src="/img/avatars/ava.jpg" alt="ava"/>
                <div className={style.time}>{msgData.currentTime}</div>
            </div>
            <div className={style.body}>
                <div className={style.messageHead}>
                    <div className={style.username}>{msgData.userName}</div>
                    <span className={`${style.menu} material-icons-outlined`}>
                        more_vert
                    </span>
                </div>
                <div className={style.text}>{msgData.text}</div>
            </div>
        </div>
    )
}
