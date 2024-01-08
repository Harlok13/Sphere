import React, {FC} from "react";
import style from "./DropDownList.module.css";
import cn from "classnames";
import {MdGroupRemove} from "react-icons/md";
import {PiCrownSimpleBold} from "react-icons/pi";
import {ParticipantActionsListHandlers} from "hooks/layout/global-aside/participants/use-participants";
import {ParticipantActionsModal} from "store/modals/modals.slice";
import {Transition} from "react-transition-group";

export const DropDownList: FC<{
    // children: ReactNode;
    handlers: ParticipantActionsListHandlers;
    isLeader: boolean;
    isOpen: boolean;
    timeout: number;
    participantActionModal: ParticipantActionsModal;
}> = ({handlers, isLeader, isOpen, timeout, participantActionModal}) => {
    const {transferLeadershipHandler, kickPlayerFromRoomHandler} = handlers;
    const {playerName, positionX, positionY} = participantActionModal

    return (
        <>
            <Transition in={isOpen} timeout={timeout} onmountOnExit={true}>
                <div className={style.dropDownList} style={{left: positionX, top: positionY}}>
                    <div className={style.wrapper}>
                        <ul className={style.list}>
                            <li
                                className={cn({[style.isNotLeader]: !isLeader}, style.actionLine)}
                                onClick={kickPlayerFromRoomHandler}
                            >
                                <span>Kick <span className={style.participantName}>{playerName}</span> from room</span>
                                <MdGroupRemove className={style.icon}/>
                            </li>
                            <li
                                className={cn({[style.isNotLeader]: !isLeader}, style.actionLine)}
                                onClick={transferLeadershipHandler}
                            >
                                <span>Transfer leadership to <span className={style.participantName}>{playerName}</span></span>
                                <PiCrownSimpleBold className={style.icon}/>
                            </li>
                        </ul>
                    </div>
                </div>
            </Transition>
        </>
    )
}