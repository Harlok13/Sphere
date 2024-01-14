import {useDispatch} from "react-redux";
import {usePlayerSelector} from "store/player/use-player-selector";
import {useStartTimerHub, useStayHub} from "hooks/hub-connection/server-methods/server-methods";
import {useNavigate} from "react-router-dom";
import {IAddedPlayerResponse} from "shared/contracts/responses/added-player-response";
import {useClientMethod} from "react-use-signalr";
import {
    addNewRoom,
    removeRoom, setRoomAvatarUrl,
    updatePlayersInRoom,
    updateRoomNameInRooms,
    updateRoomStatus
} from "store/lobby/lobby.slice";
import {
    initGameHistory,
    initRoomData,
    removePlayerFromPlayers,
    setCardInPlayersCards,
    setGameStarted, setNewGameHistoryMessage,
    setNewPlayer,
    updateBankValue,
    updateInGameInPlayers,
    updateIsLeaderInPlayers,
    updateMoneyInPlayers,
    updateOnlineInPlayers,
    updatePlayersList,
    updateReadinessInPlayers,
    updateRoomNameInRoomData
} from "store/game21/game21.slice";
import {ICreatedRoomResponse} from "shared/contracts/room-in-lobby-dto";
import {
    initPlayerData,
    resetPlayerState,
    setGameMoney, setInGame, setIsLeader,
    setMove,
    setNewCard, setOnline,
    setReadiness, setRoomId, setTimer
} from "store/player/player.slice";
import {IRemovedPlayerResponse} from "shared/contracts/removed-player-response";
import {IRemovedRoomResponse} from "shared/contracts/responses/removed-room-response";
import {IChangedPlayerReadinessResponse} from "shared/contracts/responses/changed-player-readiness-response";
import {ISelectStartGameMoneyResponse} from "shared/contracts/select-start-game-money-response";
import {initSelectStartMoney} from "store/money/money.slice";
import {setReconnectToRoomModal, setSelectStartMoneyModal} from "store/modals/modals.slice";
import {IChangedPlayerInfoMoneyResponse} from "shared/contracts/responses/changed-player-info-money-response";
import {setMoney} from "store/player-info/player-info.slice";
import {INotificationResponse} from "shared/contracts/notification-response";
import {removeNotification, setNewNotification} from "store/notifications/notifications.slice";
import {IAddedCardResponse} from "shared/contracts/responses/added-card-response";
import {IChangedRoomBankResponse} from "shared/contracts/responses/changed-room-bank-response";
import {IChangedPlayerMoveResponse} from "shared/contracts/responses/changed-player-move-response";
import {IStartTimerRequest} from "shared/contracts/requests/start-timer-request";
import {IChangedPlayerMoneyResponse} from "shared/contracts/responses/changed-player-money-response";
import {IChangedPlayerInGameResponse} from "shared/contracts/responses/changed-player-in-game-response";
import {IChangedRoomStatusResponse} from "shared/contracts/responses/changed-room-status-response";
import {IChangedRoomPlayersInRoomResponse} from "shared/contracts/responses/changed-room-players-in-room-response";
import {IChangedPlayerIsLeader} from "shared/contracts/responses/changed-player-is-leader-response";
import {IChangedRoomRoomNameResponse} from "shared/contracts/responses/changed-room-room-name-response";
import {IChangedRoomAvatarUrlResponse} from "shared/contracts/responses/changed-room-avatar-response";
import {IStayRequest} from "shared/contracts/requests/stay-request";
import {IChangedPlayerOnlineResponse} from "shared/contracts/responses/changed-player-online-response";
import {IReconnectToRoomResponse} from "shared/contracts/responses/reconnect-to-room-response";
import {IReconnectingInitRoomDataResponse} from "shared/contracts/responses/reconnecting-init-room-data-response";
import {INavigateResponse} from "shared/contracts/responses/navigate-response";
import {ICreatedPlayerResponse} from "shared/contracts/responses/created-player-response";
import {signalRConnection} from "providers/SignalrProvider";
import {IAddedGameHistoryMessageResponse} from "shared/contracts/responses/added-game-history-message-response";
import {RoomStatusEnum} from "shared/constants/room-status.enum";


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

    useClientMethod(signalRConnection, "ReceiveUser_CreatedPlayer", (response: ICreatedPlayerResponse) => {
            console.log("receive user created player");
            dispatch(initPlayerData(response.player));
            dispatch(updatePlayersList(response.players));
            dispatch(initRoomData(response.initRoomData));
        });

    useClientMethod(signalRConnection, "ReceiveUser_RemoveFromRoom", () => {
        console.log("receive user remove from room");
        dispatch(resetPlayerState());
    });

    useClientMethod(signalRConnection, "ReceiveGroup_RemovedPlayer", (removedPlayerResponse: IRemovedPlayerResponse) => {
        console.log("receive removed player");
        dispatch(removePlayerFromPlayers(removedPlayerResponse));
    });

    useClientMethod(signalRConnection, "ReceiveAll_RemovedRoom", (response: IRemovedRoomResponse) => {
        console.log("receive removed room");
        dispatch(removeRoom(response));
    });

    useClientMethod(signalRConnection, "ReceiveUser_ChangedPlayerReadiness", (response: IChangedPlayerReadinessResponse) => {
        console.log("receive user changed player readiness");
        dispatch(setReadiness(response));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_ChangedPlayerReadiness", (response: IChangedPlayerReadinessResponse) => {
        console.log("receive group changed player readiness");
        dispatch(updateReadinessInPlayers(response));
    });

    useClientMethod(signalRConnection, "ReceiveUser_SelectStartGameMoney", (selector: ISelectStartGameMoneyResponse) => {
        console.log("receive user select start game money");
        dispatch(initSelectStartMoney(selector));
        dispatch(setSelectStartMoneyModal(true));
    });

    useClientMethod(signalRConnection, "ReceiveUser_ChangedPlayerInfoMoney", (response: IChangedPlayerInfoMoneyResponse) => {
        console.log("receive user changed player info money");
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

    useClientMethod(signalRConnection, "ReceiveUser_AddedCard", (response: IAddedCardResponse) => {
        console.log("receive user added card", response.cardDto);
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

    useClientMethod(signalRConnection, "ReceiveUser_ChangedPlayerMove", (response: IChangedPlayerMoveResponse) => {
        console.log("receive user changed player move");
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

    useClientMethod(signalRConnection, "ReceiveUser_ChangedPlayerMoney", (response: IChangedPlayerMoneyResponse) => {
        console.log("receive user changed player money");
        dispatch(setGameMoney(response));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_ChangedPlayerMoney", (response: IChangedPlayerMoneyResponse) => {
        console.log("receive group changed player money");
        dispatch(updateMoneyInPlayers(response));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_ChangedRoomStatus", (response: IChangedRoomStatusResponse) => {
        console.log("receive group changed room status");

        console.log(response)
        if (response.status === RoomStatusEnum.Playing){
            dispatch(setGameStarted(true));
            console.log("invoke")
        }
        else {
            dispatch(setGameStarted(false));
        }
    });

    useClientMethod(signalRConnection, "ReceiveUser_ChangedTimer", (seconds: number) => {
        console.log("receive user changed timer");
        dispatch(setTimer(seconds));
    });

    useClientMethod(signalRConnection, "ReceiveUser_ChangedPlayerInGame", (response: IChangedPlayerInGameResponse) => {
        console.log("receive user changed player in game");
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

    useClientMethod(signalRConnection, "ReceiveUser_ChangedPlayerIsLeader", (response: IChangedPlayerIsLeader) => {
        console.log("receive user changed player is leader");
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

    useClientMethod(signalRConnection, "ReceiveUser_TimeOut", () => {
        console.log("receive user time out")
        const request: IStayRequest = {
            playerId: player.id,
            roomId: player.roomId
        }
        stay
            .invoke(request)
            .catch(err => console.error(err.toString()));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_ChangedPlayerOnline", (response: IChangedPlayerOnlineResponse) => {
        console.log("receive group changed player online", response);
        dispatch(updateOnlineInPlayers(response));
    });

    useClientMethod(signalRConnection, "ReceiveUser_ChangedPlayerOnline", (response: IChangedPlayerOnlineResponse) => {
        console.log("receive user changed player online", response);
        dispatch(setOnline(response));
    });

    useClientMethod(signalRConnection, "ReceiveUser_ReconnectToRoom", (response: IReconnectToRoomResponse) => {
        console.log("receive user reconnect to room");
        dispatch(setReconnectToRoomModal(true));
        dispatch(setRoomId(response));
    });

    useClientMethod(signalRConnection, "ReceiveUser_ReconnectingInitRoomData", (response: IReconnectingInitRoomDataResponse) => {
        console.log("receive user reconnecting init room data");
        dispatch(initPlayerData(response.player));
        dispatch(initRoomData(response.initRoomData));
        dispatch(updatePlayersList(response.players));
        dispatch(initGameHistory(response.gameHistory));
        console.log(response.gameHistory);
    });

    useClientMethod(signalRConnection, "ReceiveUser_Navigate", (response: INavigateResponse) => {
        console.log("receive user navigate");
        if (response.navigate === "Room"){
            response.navigate = "room/5";
        }
        navigate(response.navigate);
    });

    useClientMethod(signalRConnection, "ReceiveClient_ForbiddenTransferLeadershipNotification", (response: INotificationResponse) => {
        console.log("receive client forbidden transfer leadership notification");
        dispatch(setNewNotification(response));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_AddedGameHistoryMessage", (response: IAddedGameHistoryMessageResponse) => {
        console.log("receive group added game history message")
        dispatch(setNewGameHistoryMessage(response));
    });
}
