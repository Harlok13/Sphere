import {UserInfo} from "shared/pages/main-page/Aside/UserInfo/UserInfo";
import {UserInfoAvatar} from "shared/pages/main-page/Aside/UserInfo/UserInfoAvatar/UserInfoAvatar";
import {UserInfoBody} from "shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoBody";
import {UserInfoHead} from "shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoHead/UserInfoHead";
import {UserName} from "shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoHead/UserName/UserName";
import {Logout} from "shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoHead/Logout/Logout";
import {UserInfoItems} from "shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoItems/UserInfoItems";
import {Money} from "shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoItems/Money/Money";
import {Level} from "shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoItems/Level/Level";
import {History} from "shared/pages/main-page/Aside/History/History";
import {HistoryHead} from "shared/pages/main-page/Aside/History/HistoryHead/HistoryHead";
import {HistoryBody} from "shared/pages/main-page/Aside/History/HistoryBody/HistoryBody";
import {UserHistoryMsg} from "shared/pages/main-page/Aside/History/HistoryBody/UserHistoryMsg/UserHistoryMsg";
import {
    Game21HistoryMsg
} from "shared/pages/main-page/Aside/History/HistoryBody/UserHistoryMsg/Game21HistoryMsg/Game21HistoryMsg";
import {HistoryShowMore} from "shared/pages/main-page/Aside/History/HistoryShowMore/HistoryShowMore";
import {Participants} from "shared/pages/main-page/Aside/Participants/Participants";
import {ParticipantsTitle} from "shared/pages/main-page/Aside/Participants/ParticipantsTitle/ParticipantsTitle";
import {ParticipantsList} from "shared/pages/main-page/Aside/Participants/ParticipantsList/ParticipantsList";
import {Participant} from "shared/pages/main-page/Aside/Participants/ParticipantsList/Participant/Participant";
import {RoomControlPanel} from "shared/pages/main-page/Aside/Participants/RoomControlPanel/RoomControlPanel";
import {LeaveButton} from "shared/pages/main-page/Aside/Participants/RoomControlPanel/LeaveButton/LeaveButton";
import {InviteButton} from "shared/pages/main-page/Aside/Participants/RoomControlPanel/InviteButton/InviteButton";
import {Aside} from "shared/pages/main-page/Aside/Aside";
import {v4} from "uuid";
import {usePlayerInfoSelector} from "BL/slices/player-info/use-player-info-selector";
import {
    useGame21PlayersSelector,
    useInRoomSelector, useRoomDataSelector
} from "BL/slices/game21/use-game21-selector";
import {useDispatch} from "react-redux";
import {usePlayerActionModalSelector} from "BL/slices/modals/use-modals-selector";
import React from "react";
import {PlayerActionModal, resetModalsState, setPlayerActionModal} from "BL/slices/modals/modals.slice";
import {PlayerActionsList} from "shared/components/modals/PlayerActionsModal/PlayerActionsList/PlayerActionsList";
import {usePlayerSelector} from "BL/slices/player/use-player-selector";
import {
    useKickPlayerFromRoomHub, useRemoveFromRoomHub,
    useTransferLeadershipHub
} from "BL/hooks/hub-connection/server-methods/server-methods";
import {IKickPlayerFromRoomRequest} from "contracts/requests/kick-player-from-room-request";
import {ITransferLeadershipRequest} from "contracts/requests/transfer-leadership-request";
import {IRemoveFromRoomRequest} from "contracts/requests/remove-from-room-request";



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

djkjklkoijkjkll;lkllklolklklkdljkimhjkikjoihfghggyuikjmkkjkioghnbhghiiowrekmllmjujkikcvghnkkkgbfgcvgfiujfgfgvbhoihjkjjhhmjkjyhgtryihjiughythgghbvbnbhjtjghhjhbnhjghnhbnhjnjhjhkvg76nbumuymklkk;tytlykktiughfgggfjhhjnmhghhhjvbvhgiu8hhtgfhghgmjhjjhuklklklkjgfgjghvvggfjierijumnhghgjoiokjgkllfiofvghhoikkiiukjkikjjiugiojkkljutyi8iouhyyughgh,m,mgghyuiojckiklklkjkiuffokjcdsxfdjlkjueroijkjjjhjkkjdiokidkltjftrjejwkjk