import {MdGroupRemove} from "react-icons/md";
import {PiCrownSimpleBold} from "react-icons/pi";
import {FC} from "react";
import {PlayerActionsListHandlers} from "components/layout/GlobalAside/GlobalAside";
import cn from "classnames";
import style from "./PlayerActionsList.module.css";

interface PlayerActionListProps {
    handlers: PlayerActionsListHandlers;
    isLeader: boolean;
}

export const PlayerActionsList: FC<PlayerActionListProps> = ({handlers, isLeader}) => {

    const {transferLeadershipHandler, kickPlayerFromRoomHandler} = handlers;

    return (
        <ul>
            <li className={cn({[style.isNotLeader]: !isLeader})} onClick={kickPlayerFromRoomHandler}>
                <span>Kick from room</span>
                <MdGroupRemove/>
            </li>
            <li className={cn({[style.isNotLeader]: !isLeader})} onClick={transferLeadershipHandler}>
                <span>Transfer leadership</span>
                <PiCrownSimpleBold/>
            </li>
        </ul>
    )
}
