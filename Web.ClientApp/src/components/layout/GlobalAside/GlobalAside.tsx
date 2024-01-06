import {useGame21PlayersSelector, useInRoomSelector, useRoomDataSelector} from "store/game21/use-game21-selector";
import {usePlayerSelector} from "store/player/use-player-selector";
import {usePlayerInfoSelector} from "store/player-info/use-player-info-selector";
import {usePlayerActionModalSelector} from "store/modals/use-modals-selector";
import {useDispatch} from "react-redux";
import {
    useKickPlayerFromRoomHub,
    useRemoveFromRoomHub,
    useTransferLeadershipHub
} from "hooks/hub-connection/server-methods/server-methods";
import {PlayerActionModal, resetModalsState, setPlayerActionModal} from "store/modals/modals.slice";
import React from "react";
import {IKickPlayerFromRoomRequest} from "shared/contracts/requests/kick-player-from-room-request";
import {ITransferLeadershipRequest} from "shared/contracts/requests/transfer-leadership-request";
import {IRemoveFromRoomRequest} from "shared/contracts/requests/remove-from-room-request";
import {Aside} from "components/shared/pages/main-page/Aside/Aside";
import {UserInfo} from "components/shared/pages/main-page/Aside/UserInfo/UserInfo";
import {UserInfoAvatar} from "components/shared/pages/main-page/Aside/UserInfo/UserInfoAvatar/UserInfoAvatar";
import {UserInfoBody} from "components/shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoBody";
import {UserInfoHead} from "components/shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoHead/UserInfoHead";
import {UserName} from "components/shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoHead/UserName/UserName";
import {Logout} from "components/shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoHead/Logout/Logout";
import {UserInfoItems} from "components/shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoItems/UserInfoItems";
import {Money} from "components/shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoItems/Money/Money";
import {Level} from "components/shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoItems/Level/Level";
import {History} from "components/shared/pages/main-page/Aside/History/History";
import {HistoryHead} from "components/shared/pages/main-page/Aside/History/HistoryHead/HistoryHead";
import {HistoryBody} from "components/shared/pages/main-page/Aside/History/HistoryBody/HistoryBody";
import {
    UserHistoryMsg
} from "components/shared/pages/main-page/Aside/History/HistoryBody/UserHistoryMsg/UserHistoryMsg";
import {
    Game21HistoryMsg
} from "components/shared/pages/main-page/Aside/History/HistoryBody/UserHistoryMsg/Game21HistoryMsg/Game21HistoryMsg";
import {HistoryShowMore} from "components/shared/pages/main-page/Aside/History/HistoryShowMore/HistoryShowMore";
import {Participants} from "components/shared/pages/main-page/Aside/Participants/Participants";
import {
    ParticipantsTitle
} from "components/shared/pages/main-page/Aside/Participants/ParticipantsTitle/ParticipantsTitle";
import {ParticipantsList} from "components/shared/pages/main-page/Aside/Participants/ParticipantsList/ParticipantsList";
import {
    PlayerActionsList
} from "components/shared/components/modals/PlayerActionsModal/PlayerActionsList/PlayerActionsList";
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
import {v4} from "uuid";


export type PlayerActionsListHandlers = {
    kickPlayerFromRoomHandler: (e: React.MouseEvent<HTMLLIElement>) => Promise<void>;
    transferLeadershipHandler: (e: React.MouseEvent<HTMLLIElement>) => Promise<void>;
}

export const GlobalAside = () => {
    const players = useGame21PlayersSelector();
    const player = usePlayerSelector();
    const inRoom = useInRoomSelector();
    const playerInfo = usePlayerInfoSelector();
    const roomData = useRoomDataSelector();
    const playerActionModal = usePlayerActionModalSelector();
    const dispatch = useDispatch();
    const removeFromRoom = useRemoveFromRoomHub();

    const kickPlayerFromRoom = useKickPlayerFromRoomHub();
    const transferLeadership = useTransferLeadershipHub();

    const playerActionModalHandler = (e: React.MouseEvent<HTMLLIElement>, selectedPlayerId: string) => {
        e.preventDefault();

        let showModal = true;
        let id = selectedPlayerId;
        if (playerActionModal.playerId === selectedPlayerId || selectedPlayerId === player.id) {
            showModal = false;
            id = "";
        }

        const data: PlayerActionModal = {
            playerId: id,
            showModal: showModal
        }
        dispatch(setPlayerActionModal(data));
    }

    const kickPlayerFromRoomHandler = async (e: React.MouseEvent<HTMLLIElement>) => {
        e.preventDefault();

        const request: IKickPlayerFromRoomRequest = {
            kickedPlayerId: playerActionModal.playerId,
            initiatorId: player.id,
            roomId: player.roomId
        }

        const response = await kickPlayerFromRoom
            .invoke(request)
            .catch(err => console.error(err.toString()));

        console.log(response);
        if (response) dispatch(resetModalsState());
    }

    const transferLeadershipHandler = async (e: React.MouseEvent<HTMLLIElement>) => {
        e.preventDefault();

        const request: ITransferLeadershipRequest = {
            senderId: player.id,
            receiverId: playerActionModal.playerId,
            roomId: player.roomId
        }

        const response = await transferLeadership
            .invoke(request)
            .catch(err => console.error(err.toString()));

        if (response) dispatch(resetModalsState())
    }

    const handlers: PlayerActionsListHandlers = {
        kickPlayerFromRoomHandler: kickPlayerFromRoomHandler,
        transferLeadershipHandler: transferLeadershipHandler
    }

    const removeFromRoomHandler = async (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();

        const request: IRemoveFromRoomRequest = {
            roomId: player.roomId,
            playerId: player.id
        }

        await removeFromRoom
            .invoke(request)
            .catch(err => console.error(err.toString()));
    }

    return (
        <Aside>
            <UserInfo>
                <UserInfoAvatar avatarUrl={playerInfo.avatarUrl}/>
                <UserInfoBody>
                    <UserInfoHead>
                        <UserName name={playerInfo.playerName}/>
                        <Logout/>
                    </UserInfoHead>
                    <UserInfoItems>
                        <Money money={playerInfo.money}/>
                        <Level playerInfo={playerInfo}/>
                    </UserInfoItems>
                </UserInfoBody>
            </UserInfo>
            <History>
                <HistoryHead/>
                <HistoryBody>
                    <UserHistoryMsg gameResult="Win">
                        <Game21HistoryMsg gameResult={"Win"} userName={"Harlok"} opponentName={"Bot"} score={"21:17"}/>
                    </UserHistoryMsg>
                </HistoryBody>
                <HistoryShowMore/>
            </History>
            {inRoom
                ? (<Participants>
                    <ParticipantsTitle roomName={roomData.roomName}/>
                    <ParticipantsList>
                        {playerActionModal.showModal
                        ? (<PlayerActionsList isLeader={player.isLeader} handlers={handlers}/>)
                        : null}
                        {players.length
                            ? players.map(p => (
                                <Participant
                                    key={v4()}
                                    playerData={p}
                                    playerActionModalHandler={playerActionModalHandler}
                                />))
                            : null}
                    </ParticipantsList>
                    <RoomControlPanel>
                        <LeaveButton removeFromRoomHandler={removeFromRoomHandler}/>
                        <InviteButton/>
                    </RoomControlPanel>
                </Participants>)
                : null}
        </Aside>
    )
}