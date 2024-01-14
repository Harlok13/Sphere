import {Participants} from "components/shared/pages/main-page/Aside/Participants/Participants";
import {
    ParticipantsTitle
} from "components/shared/pages/main-page/Aside/Participants/ParticipantsTitle/ParticipantsTitle";
import {ParticipantsList} from "components/shared/pages/main-page/Aside/Participants/ParticipantsList/ParticipantsList";
import {
    Participant
} from "components/shared/pages/main-page/Aside/Participants/ParticipantsList/Participant/Participant";
import {RoomControlPanel} from "components/shared/pages/main-page/Aside/Participants/RoomControlPanel/RoomControlPanel";
import {
    LeaveButton
} from "components/shared/pages/main-page/Aside/Participants/RoomControlPanel/LeaveButton/LeaveButton";
import {
    InviteButton
} from "components/shared/pages/main-page/Aside/Participants/RoomControlPanel/InviteButton/InviteButton";
import React, {useRef} from "react";
import {v4} from "uuid";
import {useParticipants} from "hooks/layout/global-aside/participants/use-participants";
import {DropDownList} from "components/shared/components/DropDownList/DropDownList";

export const GlobalParticipants = () => {
    // const dropDownListRef = useRef<HTMLDivElement>();
    const {
        roomData,
        inRoom,
        participantActionsModal,
        player,
        players,
        participantActionsModalHandler,
        handlers,
        removeFromRoomHandler,
        // dropDownListRef,
    } = useParticipants();

    return (
        <>
            {inRoom
                ? (<Participants>
                    <ParticipantsTitle roomName={roomData.roomName}/>
                    <ParticipantsList>
                        {participantActionsModal.showModal && <DropDownList
                                handlers={handlers}
                                isLeader={player.isLeader}
                                participantActionModal={participantActionsModal}
                                isOpen={participantActionsModal.showModal}
                                timeout={350}
                            />}
                        {players.length > 0 && players.map(p => (
                                <Participant
                                    key={v4()}
                                    playerData={p}
                                    participantActionsModalHandler={participantActionsModalHandler}
                                    // dropDownListRef={dropDownListRef}
                                />))}
                    </ParticipantsList>
                    <RoomControlPanel>
                        <LeaveButton removeFromRoomHandler={removeFromRoomHandler}/>
                        <InviteButton/>
                    </RoomControlPanel>
                </Participants>)
                : null}
        </>
    )
}