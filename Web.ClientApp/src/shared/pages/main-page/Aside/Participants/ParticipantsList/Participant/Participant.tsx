import style from "./Participant.module.css";
import {Player} from "contracts/player-dto";
import React, {FC} from "react";
import { VscDebugDisconnect } from "react-icons/vsc";
import cn from "classnames";

interface IParticipantProps {
    playerData: Player;
    playerActionModalHandler: (e: React.MouseEvent<HTMLLIElement>, playerId: string) => void;
}

export const Participant: FC<IParticipantProps> = ({playerData, playerActionModalHandler}) => {
    return (
        <li
            onClick={(e) => playerActionModalHandler(e, playerData.id)}
            className={cn(style.item, {[style.inactive]: !playerData.online})}
        >
            {playerData.online
                ? null
                : (<VscDebugDisconnect className={style.disconnect}/>)}

            <span className={cn(style.iconLeader, "material-icons-outlined")}>
                {playerData.isLeader ? "star" : null}
            </span>
            {playerData.playerName}
            <span className={cn(style.iconReadiness, "material-icons-outlined")}>
                {playerData.readiness ? "done" : "close"}
            </span>
        </li>
    )
}