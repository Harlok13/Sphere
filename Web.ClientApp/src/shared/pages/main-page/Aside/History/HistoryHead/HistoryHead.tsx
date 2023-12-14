// @ts-ignore
import style from "./HistoryHead.module.css";

const HistoryHead = () => {
    return (
        <div className={style.head}>
            <div className={style.title}>History</div>
            <span className={style.clear_text}>Clear</span>
            <span className={`${style.clear} material-icons-outlined`}>delete_forever</span>
        </div>
    )
}

export {HistoryHead};