import {useDispatch} from "react-redux";
import {
    addNewRoom,
    removeRoom, setRoomAvatarUrl,
    updatePlayersInRoom,
    updateRoom, updateRoomNameInRooms,
    updateRoomStatus
} from "BL/slices/lobby/lobby.slice";
import {
    initRoomData,
    removePlayerFromPlayers, setCardInPlayersCards, setGameStarted,
    setNewPlayer, updateBankValue, updateInGameInPlayers, updateIsLeaderInPlayers, updateMoneyInPlayers,
    updatePlayerInPlayers,
    updatePlayersList, updateReadinessInPlayers, updateRoomName, updateRoomNameInRoomData,
} from "BL/slices/game21/game21.slice";
import {useClientMethod} from "react-use-signalr";
import {signalRConnection} from "../../../App";
import {
    initPlayerData,
    resetState,
    setGameMoney, setInGame,
    setIsLeader,
    setMove, setNewCard,
    setReadiness, setTimer
} from "BL/slices/player/player.slice";
import {IPlayerResponse} from "contracts/player-response";
import {ICreatedRoomResponse, IRoomInLobbyDto} from "contracts/room-in-lobby-response";
import {usePlayerSelector} from "BL/slices/player/use-player-selector";
import {ISelectStartGameMoneyResponse} from "contracts/select-start-game-money-response";
import {initSelectStartMoney, setShowModal} from "BL/slices/money/money.slice";
import {setMoney} from "BL/slices/player-info/player-info.slice";
import {INotEnoughMoneyNotificationResponse} from "contracts/not-enough-money-notification-response";
import {removeNotification, setNewNotification} from "BL/slices/notifications/notifications";
import {IAddedCardResponse, ICardDto} from "contracts/added-card-response";
import {useStartTimerHub} from "BL/hooks/hub-connection/server-methods/server-methods";
import {IStartTimerRequest} from "contracts/requests/start-timer-request";
import {IUpdatedRoomStatusResponse} from "contracts/updated-room-status-response";
import {IUpdatedRoomPlayersInRoomResponse} from "contracts/updated-room-players-in-room-response";
import {IRemovedPlayerResponse} from "contracts/removed-player-from-response";
import {IUpdatedPlayerIsLeader} from "contracts/updated-player-is-leader";
import {IChangedRoomRoomNameResponse} from "contracts/changed-room-room-name-response";
import {IChangedRoomAvatarUrlResponse} from "../../../contracts/changed-room-avatar-response";
import {IChangedPlayerReadinessResponse} from "../../../contracts/changed-player-readiness-response";
import {IChangedPlayerMoneyResponse} from "../../../contracts/responses/changed-player-money-response";
import {IChangedPlayerInGameResponse} from "../../../contracts/responses/changed-player-in-game-response";
import {IChangedRoomBankResponse} from "../../../contracts/responses/changed-room-bank-response";
import {IChangedPlayerMoveResponse} from "../../../contracts/responses/changed-player-move-response";

export const useGlobalHubConnection = () => {
    const dispatch = useDispatch();
    const player = usePlayerSelector();
    const startTimer = useStartTimerHub();

    useClientMethod(signalRConnection, "ReceiveGroup_NewPlayer", async (newPlayer: IPlayerResponse) => {
        console.log("receive new player");
        dispatch(setNewPlayer(newPlayer));
    });

    useClientMethod(signalRConnection, "ReceiveAll_CreatedRoom", (response: ICreatedRoomResponse) => {
        console.log("receive all created room");
        dispatch(addNewRoom(response));
    });

    // useClientMethod(signalRConnection, "ReceiveAll_UpdatedRoom", (updatedRoom: IRoomInLobbyDto) => {
    //     console.log("receive updated room");
    //     dispatch(updateRoom(updatedRoom));
    // });

    useClientMethod(
        signalRConnection,
        "ReceiveOwn_PlayerData",
        (playerData: IPlayerResponse, roomData: IRoomInLobbyDto, playersList: Array<IPlayerResponse>) => {

            console.log("receive own player data");
            dispatch(initPlayerData(playerData));
            dispatch(updatePlayersList(playersList));
            dispatch(initRoomData(roomData));
        });

    useClientMethod(signalRConnection, "ReceiveOwn_RemoveFromRoom", () => {
        console.log("receive remove from room");
        dispatch(resetState());
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

    useClientMethod(signalRConnection, "ReceiveAll_RemovedRoom", (roomId: string) => {
        console.log("receive removed room");
        dispatch(removeRoom(roomId));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_UpdatedPlayer", (updatedPlayer: IPlayerResponse) => {
        console.log("receive updated player");
        dispatch(updatePlayerInPlayers(updatedPlayer));
    });

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
        dispatch(setShowModal(true));
    });

    useClientMethod(signalRConnection, "ReceiveOwn_UpdatedMoney", (moneyValue: number) => {
        console.log("receive updated money");
        dispatch(setMoney(moneyValue));
    });

    useClientMethod(signalRConnection, "ReceiveOwn_NotEnoughMoneyNotification", (notification: INotEnoughMoneyNotificationResponse) => {
        console.log("receive not enough money notification");
        dispatch(setNewNotification(notification));

        setTimeout(() => {
            dispatch(removeNotification(notification.notificationId))
        }, 5000);
    });

    useClientMethod(signalRConnection, "ReceiveGroup_NewRoomName", (newRoomName: string) => {
        console.log("receive new room name");
        dispatch(updateRoomName(newRoomName));
    });

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
        startTimer
            .invoke(startTimerRequest)
            .catch(err => console.error(err.toString()));
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

    useClientMethod(signalRConnection, "ReceiveAll_UpdatedRoomStatus", (response: IUpdatedRoomStatusResponse) => {
        console.log("receive all updated room status");
        dispatch(updateRoomStatus(response));
    });

    useClientMethod(signalRConnection, "ReceiveAll_UpdatedRoomPlayersInRoom", (response: IUpdatedRoomPlayersInRoomResponse) => {
        console.log("receive updated room players in room");
        dispatch(updatePlayersInRoom(response));
    });

    useClientMethod(signalRConnection, "ReceiveOwn_UpdatedPlayerIsLeader", (response: IUpdatedPlayerIsLeader) => {
        console.log("receive own updated player is leader");
        dispatch(setIsLeader(response));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_UpdatedPlayerIsLeader", (response: IUpdatedPlayerIsLeader) => {
        console.log("receive group updated player is leader");
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
}

export type PlayerInGame = {
    playerId: string;
    inGame: boolean;
}