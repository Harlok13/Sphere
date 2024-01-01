import {useDispatch} from "react-redux";
import {
    addNewRoom,
    removeRoom, setRoomAvatarUrl,
    updatePlayersInRoom,
    updateRoomNameInRooms,
    updateRoomStatus
} from "BL/slices/lobby/lobby.slice";
import {
    initRoomData,
    removePlayerFromPlayers,
    setCardInPlayersCards,
    setGameStarted,
    setNewPlayer,
    updateBankValue,
    updateInGameInPlayers,
    updateIsLeaderInPlayers,
    updateMoneyInPlayers,
    updateOnlineInPlayers,
    updatePlayersList,
    updateReadinessInPlayers,
    updateRoomNameInRoomData,
} from "BL/slices/game21/game21.slice";
import {useClientMethod} from "react-use-signalr";
import {signalRConnection} from "App";
import {
    initPlayerData, resetPlayerState,
    setGameMoney, setInGame,
    setIsLeader,
    setMove, setNewCard, setOnline,
    setReadiness, setRoomId, setTimer
} from "BL/slices/player/player.slice";
import {ICreatedRoomResponse} from "contracts/room-in-lobby-dto";
import {usePlayerSelector} from "BL/slices/player/use-player-selector";
import {ISelectStartGameMoneyResponse} from "contracts/select-start-game-money-response";
import {initSelectStartMoney} from "BL/slices/money/money.slice";
import {setMoney} from "BL/slices/player-info/player-info.slice";
import {INotificationResponse} from "contracts/notification-response";
import {removeNotification, setNewNotification} from "BL/slices/notifications/notifications";
import {IAddedCardResponse} from "contracts/responses/added-card-response";
import {useStartTimerHub, useStayHub} from "BL/hooks/hub-connection/server-methods/server-methods";
import {IStartTimerRequest} from "contracts/requests/start-timer-request";
import {IChangedRoomStatusResponse} from "contracts/responses/changed-room-status-response";
import {IRemovedPlayerResponse} from "contracts/removed-player-response";
import {IChangedPlayerIsLeader} from "contracts/responses/changed-player-is-leader-response";
import {IChangedRoomRoomNameResponse} from "contracts/responses/changed-room-room-name-response";
import {IChangedRoomAvatarUrlResponse} from "contracts/responses/changed-room-avatar-response";
import {IChangedPlayerReadinessResponse} from "contracts/responses/changed-player-readiness-response";
import {IChangedPlayerMoneyResponse} from "contracts/responses/changed-player-money-response";
import {IChangedPlayerInGameResponse} from "contracts/responses/changed-player-in-game-response";
import {IChangedRoomBankResponse} from "contracts/responses/changed-room-bank-response";
import {IChangedPlayerMoveResponse} from "contracts/responses/changed-player-move-response";
import {ICreatedPlayerResponse} from "contracts/responses/created-player-response";
import {IChangedPlayerInfoMoneyResponse} from "contracts/responses/changed-player-info-money-response";
import {IAddedPlayerResponse} from "contracts/responses/added-player-response";
import {IChangedRoomPlayersInRoomResponse} from "contracts/responses/changed-room-players-in-room-response";
import {IStayRequest} from "contracts/requests/stay-request";
import {IChangedPlayerOnlineResponse} from "contracts/responses/changed-player-online-response";
import {setReconnectToRoomModal, setSelectStartMoneyModal} from "../../slices/modals/modals.slice";
import {IReconnectToRoomResponse} from "../../../contracts/responses/reconnect-to-room-response";
import {IReconnectingInitRoomDataResponse} from "../../../contracts/responses/reconnecting-init-room-data-response";
import {useNavigate} from "react-router-dom";
import {NavigateEnum} from "../../../constants/navigate.enum";
import {IRemovedRoomResponse} from "contracts/responses/removed-room-response";

export const useGlobalHubConnection = () => {
    const dispatch = useDispatch();
    const player = usePlayerSelector();
    const startTimer = useStartTimerHub();
    const stay = useStayHub();
    const navigate = useNavigate();

    useClientMethod(signalRConnection, "ReceiveGroup_AddedPlayer", (response: IAddedPlayerResponse) => {
        console.log("receive new player");
        dispatch(setNewPlayer(response));
    });

    useClientMethod(signalRConnection, "ReceiveAll_CreatedRoom", (response: ICreatedRoomResponse) => {
        console.log("receive all created room");
        dispatch(addNewRoom(response));
    });

    // useClientMethod(signalRConnection, "ReceiveAll_UpdatedRoom", (updatedRoom: IRoomInLobbyDto) => {
    //     console.log("receive updated room");
    //     dispatch(updateRoom(updatedRoom));
    // });

    useClientMethod(signalRConnection, "ReceiveOwn_CreatedPlayer", (response: ICreatedPlayerResponse) => {
            console.log("receive own created player");
            dispatch(initPlayerData(response.player));
            dispatch(updatePlayersList(response.players));
            dispatch(initRoomData(response.initRoomData));
        });

    useClientMethod(signalRConnection, "ReceiveOwn_RemoveFromRoom", () => {
        console.log("receive remove from room");
        dispatch(resetPlayerState());
    });

    useClientMethod(signalRConnection, "ReceiveGroup_RemovedPlayer", (removedPlayerResponse: IRemovedPlayerResponse) => {
        console.log("receive removed player");
        dispatch(removePlayerFromPlayers(removedPlayerResponse));
    });

    // useClientMethod(signalRConnection, "ReceiveGroup_NewRoomLeader", (newRoomLeader: IPlayerResponse) => {
    //     console.log("receive new room leader");
    //     dispatch(updatePlayerInPlayers(newRoomLeader));
    //     if (player.id === newRoomLeader.id) {  // TODO: remove hard code. receive in extra client method
    //         dispatch(setIsLeader());
    //     }
    // });

    useClientMethod(signalRConnection, "ReceiveAll_RemovedRoom", (response: IRemovedRoomResponse) => {
        console.log("receive removed room");
        dispatch(removeRoom(response));
    });

    // useClientMethod(signalRConnection, "ReceiveGroup_UpdatedPlayer", (updatedPlayer: IPlayerDto) => {
    //     console.log("receive updated player");
    //     dispatch(updatePlayerInPlayers(updatedPlayer));
    // });

    useClientMethod(signalRConnection, "ReceiveOwn_ChangedPlayerReadiness", (response: IChangedPlayerReadinessResponse) => {
        console.log("receive own changed player readiness");
        dispatch(setReadiness(response));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_ChangedPlayerReadiness", (response: IChangedPlayerReadinessResponse) => {
        console.log("receive group changed player readiness");
        dispatch(updateReadinessInPlayers(response));
    });

    useClientMethod(signalRConnection, "ReceiveOwn_SelectStartGameMoney", (selector: ISelectStartGameMoneyResponse) => {
        console.log("receive select start game money");
        dispatch(initSelectStartMoney(selector));
        dispatch(setSelectStartMoneyModal(true));
    });

    useClientMethod(signalRConnection, "ReceiveOwn_ChangedPlayerInfoMoney", (response: IChangedPlayerInfoMoneyResponse) => {
        console.log("receive changed player info money");
        dispatch(setMoney(response));
    });

    useClientMethod(signalRConnection, "ReceiveClient_Notification", (notification: INotificationResponse) => {
        console.log("receive client notification - ", notification.notificationText);
        dispatch(setNewNotification(notification));

        setTimeout(() => {
            dispatch(removeNotification(notification.notificationId))
        }, 5000);
    });

    useClientMethod(signalRConnection, "ReceiveUser_Notification", (notification: INotificationResponse) => {
        console.log("receive user notification - ", notification.notificationText);
        dispatch(setNewNotification(notification));

        setTimeout(() => {
            dispatch(removeNotification(notification.notificationId))
        }, 5000);
    });

    // useClientMethod(signalRConnection, "ReceiveGroup_NewRoomName", (newRoomName: string) => {
    //     console.log("receive new room name");
    //     dispatch(updateRoomName(newRoomName));
    // });

    useClientMethod(signalRConnection, "ReceiveOwn_AddedCard", (response: IAddedCardResponse) => {
        console.log("receive own added card", response.cardDto);
        dispatch(setNewCard(response));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_AddedCard", (response: IAddedCardResponse) => {
        console.log("receive group added card", response.cardDto);
        dispatch(setCardInPlayersCards(response));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_ChangedRoomBank", (response: IChangedRoomBankResponse) => {
        console.log("receive group changed room bank");
        dispatch(updateBankValue(response));
    });

    useClientMethod(signalRConnection, "ReceiveOwn_ChangedPlayerMove", (response: IChangedPlayerMoveResponse) => {
        console.log("receive own changed player move");
        dispatch(setMove(response));

        const startTimerRequest: IStartTimerRequest = {
            roomId: player.roomId,
            playerId: player.id
        }

        if (response.move){
            startTimer
                .invoke(startTimerRequest)
                .catch(err => console.error(err.toString()));
        }
    });


    // useClientMethod(signalRConnection, "ReceiveMoveEnd", () => {
    //     console.log("receive move end");
    //     dispatch(setMove(false));
    // })

    useClientMethod(signalRConnection, "ReceiveGroup_StartGameErrorNotification", (notification: string) => {
        // TODO: finish
    });

    useClientMethod(signalRConnection, "ReceiveOwn_ChangedPlayerMoney", (response: IChangedPlayerMoneyResponse) => {
        console.log("receive own changed player money");
        dispatch(setGameMoney(response));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_ChangedPlayerMoney", (response: IChangedPlayerMoneyResponse) => {
        console.log("receive group changed player money");
        dispatch(updateMoneyInPlayers(response));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_StartGame", () => {
        console.log("receive start game");
        dispatch(setGameStarted(true));
    });

    useClientMethod(signalRConnection, "ReceiveOwn_UpdatedTimer", (seconds: number) => {
        console.log("receive updated timer");
        dispatch(setTimer(seconds));
    });

    useClientMethod(signalRConnection, "ReceiveOwn_ChangedPlayerInGame", (response: IChangedPlayerInGameResponse) => {
        console.log("receive own changed player in game");
        dispatch(setInGame(response));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_ChangedPlayerInGame", (response: IChangedPlayerInGameResponse) => {
        console.log("receive group changed player in game")
        dispatch(updateInGameInPlayers(response));
    });

    useClientMethod(signalRConnection, "ReceiveAll_ChangedRoomStatus", (response: IChangedRoomStatusResponse) => {
        console.log("receive all updated room status");
        dispatch(updateRoomStatus(response));
    });

    useClientMethod(signalRConnection, "ReceiveAll_ChangedRoomPlayersInRoom", (response: IChangedRoomPlayersInRoomResponse) => {
        console.log("receive changed room players in room");
        dispatch(updatePlayersInRoom(response));
    });

    useClientMethod(signalRConnection, "ReceiveOwn_ChangedPlayerIsLeader", (response: IChangedPlayerIsLeader) => {
        console.log("receive own changed player is leader");
        dispatch(setIsLeader(response));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_ChangedPlayerIsLeader", (response: IChangedPlayerIsLeader) => {
        console.log("receive group changed player is leader");
        dispatch(updateIsLeaderInPlayers(response));
    });

    useClientMethod(signalRConnection, "ReceiveAll_ChangedRoomRoomName", (response: IChangedRoomRoomNameResponse) => {
        console.log("receive all changed room room name");
        dispatch(updateRoomNameInRooms(response));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_ChangedRoomRoomName", (response: IChangedRoomRoomNameResponse) => {
        console.log("receive group changed room room name");
        dispatch(updateRoomNameInRoomData(response));
    });

    useClientMethod(signalRConnection, "ReceiveAll_ChangedRoomAvatarUrl", (response: IChangedRoomAvatarUrlResponse) => {
        console.log("receive group changed room avatar url");
        dispatch(setRoomAvatarUrl(response));
    });

    // useClientMethod(signalRConnection, "ReceiveGroup_PlayerInGame", (playerId: string, inGame: boolean) => {
    //     console.log("receive player in game");
    //     // TODO: is redundant?
    //
    // });

    useClientMethod(signalRConnection, "ReceiveClient_TimeOut", () => {
        console.log("receive client time out")
        const request: IStayRequest = {
            playerId: player.id,
            roomId: player.roomId
        }
        stay
            .invoke(request)
            .catch(err => console.error(err.toString()));
    });

    // useClientMethod(signalRConnection, "ReceiveGroup_DisconnectedPlayer", (response: IDisconnectedPlayerResponse) => {
    //     console.log("receive group disconnected player - ", response.playerId);
    // });
    //
    // useClientMethod(signalRConnection, "ReceiveGroup_ReconnectedPlayer", (response: IReconnectedPlayerResponse) => {
    //     console.log("receive group reconnected player - ", response.playerId)
    // });

    useClientMethod(signalRConnection, "ReceiveGroup_ChangedPlayerOnline", (response: IChangedPlayerOnlineResponse) => {
        console.log("receive group changed player online", response);
        dispatch(updateOnlineInPlayers(response));
    });

    useClientMethod(signalRConnection, "ReceiveClient_ChangedPlayerOnline", (response: IChangedPlayerOnlineResponse) => {
        console.log("receive client changed player online", response);
        dispatch(setOnline(response));
    });

    useClientMethod(signalRConnection, "ReceiveUser_ReconnectToRoom", (response: IReconnectToRoomResponse) => {
        console.log("receive user reconnect to room");
        dispatch(setReconnectToRoomModal(true));
        dispatch(setRoomId(response));
    });

    useClientMethod(signalRConnection, "ReceiveClient_ReconnectingInitRoomData", (response: IReconnectingInitRoomDataResponse) => {
        console.log("receive client reconnecting init room data");
        dispatch(initPlayerData(response.player));
        dispatch(initRoomData(response.initRoomData));
        dispatch(updatePlayersList(response.players));
    });

    useClientMethod(signalRConnection, "ReceiveUser_NavigateToLobby", () => {
        console.log("receive user navigate to lobby");
        navigate(NavigateEnum.Lobby);
    });

    useClientMethod(signalRConnection, "ReceiveClient_ForbiddenTransferLeadershipNotification", (response: INotificationResponse) => {
        console.log("receive client forbidden transfer leadership notification");
        dispatch(setNewNotification(response));
    });
}

export type PlayerInGame = {
    playerId: string;
    inGame: boolean;
}
