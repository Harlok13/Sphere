import style from "../UserInfoItems.module.css";
import {PlayerInfo} from "../../../../../../../../contracts/player-info-response";
import {FC} from "react";

interface IPlayerInfoProps {
    playerInfo: PlayerInfo;
}

export const Level: FC<IPlayerInfoProps> = ({playerInfo}) => {
    return (
        <li className={style.item}>
            <span className={`${style.levelIcon} material-icons-outlined`}>keyboard_double_arrow_up</span>
            <h4 className={style.level}>{playerInfo.level}</h4>
            <meter className={style.scale} value={playerInfo.currentExp} min="0" max={playerInfo.targetExp}></meter>
        </li>
    )
}