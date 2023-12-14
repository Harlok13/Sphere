import style from "./TextArea.module.css";

export const TextArea = ({props}) => {
    return (
        <div className={style.textArea}>
            <textarea value={props.message} onChange={props.messageHandler} className={style.input} name="" ></textarea>
            <span onClick={props.sendMessage} ref={props.sendBtnRef} className={`${style.iconSend} material-icons-outlined`}>
                    send
            </span>
        </div>
    )
}