import style from "./AboutRoomItem.module.css";
import {FC} from "react";

type RoomInfo = {
    title: string;
    value: string | number;
}

interface IAboutRoomItemProps {
    roomInfoData: RoomInfo;
}

const AboutRoomItem: FC<IAboutRoomItemProps> = ({roomInfoData}) => {
    return (
        // <li className={style.item}>Size room: <span className={style.value}>2/2</span></li>
        <li className={style.item}>{roomInfoData.title}: <span className={style.value}>{roomInfoData.value}</span></li>
    )
}

export {AboutRoomItem};