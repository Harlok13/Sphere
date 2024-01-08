import {useGame21PlayersSelector, useInRoomSelector, useRoomDataSelector} from "store/game21/use-game21-selector";
import {useParticipantActionsModalSelector} from "store/modals/use-modals-selector";
import {usePlayerSelector} from "store/player/use-player-selector";
import {
    useKickPlayerFromRoomHub,
    useRemoveFromRoomHub,
    useTransferLeadershipHub
} from "hooks/hub-connection/server-methods/server-methods";
import {useDispatch} from "react-redux";
import React from "react";
import {IRemoveFromRoomRequest} from "shared/contracts/requests/remove-from-room-request";
import {ParticipantActionsModal, resetModalsState, setParticipantActionsModal} from "store/modals/modals.slice";
import {IKickPlayerFromRoomRequest} from "shared/contracts/requests/kick-player-from-room-request";
import {ITransferLeadershipRequest} from "shared/contracts/requests/transfer-leadership-request";


export type ParticipantActionsListHandlers = {
    kickPlayerFromRoomHandler: (e: React.MouseEvent<HTMLLIElement>) => Promise<void>;
    transferLeadershipHandler: (e: React.MouseEvent<HTMLLIElement>) => Promise<void>;
}

export const useParticipants = () => {
    const inRoom = useInRoomSelector();
    const participantActionsModal = useParticipantActionsModalSelector();
    const players = useGame21PlayersSelector();
    const player = usePlayerSelector();
    const removeFromRoom = useRemoveFromRoomHub();
    const roomData = useRoomDataSelector();
    const dispatch = useDispatch();
    const kickPlayerFromRoom = useKickPlayerFromRoomHub();
    const transferLeadership = useTransferLeadershipHub();

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

    const participantActionsModalHandler = (e: React.MouseEvent<HTMLLIElement>, selectedPlayerId: string, selectedPlayerName: string) => {
        e.preventDefault();

        let showModal = true;
        let id = selectedPlayerId;
        let name = selectedPlayerName;
        let positionX = e.clientX -40;
        let positionY = e.clientY - 70;
        if (participantActionsModal.playerId === selectedPlayerId || selectedPlayerId === player.id) {
            showModal = false;
            id = "";
            name = "";
            positionX = 0;
            positionY = 0;
        }
        console.log(e.clientX, e.clientY);

        const data: ParticipantActionsModal = {
            playerId: id,
            playerName: name,
            showModal: showModal,
            positionX: positionX,
            positionY: positionY
        }
        dispatch(setParticipantActionsModal(data));
    }

    const kickPlayerFromRoomHandler = async (e: React.MouseEvent<HTMLLIElement>) => {
        e.preventDefault();

        const request: IKickPlayerFromRoomRequest = {
            kickedPlayerId: participantActionsModal.playerId,
            initiatorId: player.id,
            roomId: player.roomId
        }

        const response = await kickPlayerFromRoom
            .invoke(request)
            .catch(err => console.error(err.toString()));

        if (response) dispatch(resetModalsState());
    }

    const transferLeadershipHandler = async (e: React.MouseEvent<HTMLLIElement>) => {
        e.preventDefault();

        const request: ITransferLeadershipRequest = {
            senderId: player.id,
            receiverId: participantActionsModal.playerId,
            roomId: player.roomId
        }

        const response = await transferLeadership
            .invoke(request)
            .catch(err => console.error(err.toString()));

        if (response) dispatch(resetModalsState())
    }

    const handlers: ParticipantActionsListHandlers = {
        kickPlayerFromRoomHandler: kickPlayerFromRoomHandler,
        transferLeadershipHandler: transferLeadershipHandler
    }

    return {
        roomData,
        inRoom,
        participantActionsModal,
        player,
        players,
        participantActionsModalHandler,
        handlers,
        removeFromRoomHandler,
        // dropDownListRef
    }
}