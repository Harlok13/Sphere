import style from "./Participant.module.css";
import {Player} from "../../../../../../../contracts/player-response";
import {FC} from "react";

interface IParticipantProps {
    playerData: Player;
}

export const Participant: FC<IParticipantProps> = ({playerData}) => {
    return (
        <li className={style.item}>
            <span className={`${style.iconLeader} material-icons-outlined`}>
                {playerData.isLeader ? "star" : null}
            </span>
            {playerData.playerName}
            <span className={`${style.iconReadiness} material-icons-outlined`}>
                {playerData.readiness ? "done" : "close"}
            </span>
        </li>
    )
}