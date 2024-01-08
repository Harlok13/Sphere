import style from "./Participant.module.css";
import React, {FC} from "react";
import { VscDebugDisconnect } from "react-icons/vsc";
import cn from "classnames";
import {Player} from "shared/contracts/player-dto";


export const Participant: FC<{
    playerData: Player;
    participantActionsModalHandler: (e: React.MouseEvent<HTMLLIElement>, playerId: string, playerName: string) => void;
}> = ({playerData, participantActionsModalHandler}) => {
    return (
        <li
            onClick={(e) => participantActionsModalHandler(e, playerData.id, playerData.playerName)}
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