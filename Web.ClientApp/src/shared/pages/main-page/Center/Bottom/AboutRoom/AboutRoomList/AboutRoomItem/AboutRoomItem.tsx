// @ts-ignore
import style from "./AboutRoomItem.module.css";

const AboutRoomItem = ({roomInfoData}) => {
    return (
        <li className={style.item}>Size room: <span className={style.value}>2/2</span></li>
        // <li className={style.item}>{roomInfoData.title}: <span className={style.value}>{roomInfoData.value}</span></li>
    )
}

export {AboutRoomItem};