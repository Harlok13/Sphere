// @ts-ignore
import style from "./Participant.module.css";
import {IParticipant} from "../../../../../../../interfaces/participant";

export const Participant = (data: IParticipant) => {
    return (
        <li className={style.item}>
            <span className={`${style.iconLeader} material-icons-outlined`}>
                {data.isLeader ? "star" : null}
            </span>
            {data.playerName}
            <span className={`${style.iconReadiness} material-icons-outlined`}>
                {data.readiness ? "done" : "close"}
            </span>
        </li>
    )
}