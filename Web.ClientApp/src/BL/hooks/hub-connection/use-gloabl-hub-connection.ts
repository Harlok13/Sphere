import {useDispatch} from "react-redux";
import {addNewRoom, removeRoom, updateRoom} from "../../slices/lobby/lobby.slice";
import {
    initRoomData,
    removePlayerFromPlayers, setGameStarted,
    setNewPlayer, updateBankValue,
    updatePlayerInPlayers,
    updatePlayersList, updateRoomName,
} from "../../slices/game21/game21.slice";
import {useClientMethod} from "react-use-signalr";
import {signalRConnection} from "../../../App";
import {
    initPlayerData,
    resetState,
    setGameMoney, setInGame,
    setIsLeader,
    setMove, setNewCard,
    setReadiness, setTimer
} from "../../slices/player/player.slice";
import {IPlayerResponse} from "../../../contracts/player-response";
import {IRoomInLobbyResponse} from "../../../contracts/room-in-lobby-response";
import {usePlayerSelector} from "../../slices/player/use-player-selector";
import {ISelectStartGameMoneyResponse} from "../../../contracts/select-start-game-money-response";
import {initSelectStartMoney, setShowModal} from "../../slices/money/money.slice";
import {setMoney} from "../../slices/player-info/player-info.slice";
import {INotificationResponse} from "../../../contracts/notification-response";
import {removeNotification, setNewNotification} from "../../slices/notifications/notifications";
import {ICardResponse} from "../../../contracts/card-response";
import {useStartTimerHub} from "./server-methods/server-methods";

export const useGlobalHubConnection = () => {
    const dispatch = useDispatch();
    const player = usePlayerSelector();
    const startTimer = useStartTimerHub();

    useClientMethod(signalRConnection, "ReceiveGroup_NewPlayer", async (newPlayer: IPlayerResponse) => {
        console.log("receive new player");
        dispatch(setNewPlayer(newPlayer));
    });

    useClientMethod(signalRConnection, "ReceiveAll_NewRoom", (newRoom: IRoomInLobbyResponse) => {
        console.log("receive new room");
        dispatch(addNewRoom(newRoom));
    });

    useClientMethod(signalRConnection, "ReceiveAll_UpdatedRoom", (updatedRoom: IRoomInLobbyResponse) => {
        console.log("receive updated room");
        dispatch(updateRoom(updatedRoom));
    });

    useClientMethod(
        signalRConnection,
        "ReceiveOwn_PlayerData",
        (playerData: IPlayerResponse, roomData: IRoomInLobbyResponse, playersList: Array<IPlayerResponse>) => {

        console.log("receive own player data");
        dispatch(initPlayerData(playerData));
        dispatch(updatePlayersList(playersList));
        dispatch(initRoomData(roomData));
    });

    useClientMethod(signalRConnection, "ReceiveOwn_RemoveFromRoom", () => {
        console.log("receive remove from room");
        dispatch(resetState());
    });

    useClientMethod(signalRConnection, "ReceiveGroup_RemovedPlayer", (removedPlayerId: string) => {
        console.log("receive removed player");
        dispatch(removePlayerFromPlayers(removedPlayerId));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_NewRoomLeader", (newRoomLeader: IPlayerResponse) => {
        console.log("receive new room leader");
        dispatch(updatePlayerInPlayers(newRoomLeader));
        if (player.id === newRoomLeader.id){  // TODO: remove hard code. receive in extra client method
            dispatch(setIsLeader());
        }
    });

    useClientMethod(signalRConnection, "ReceiveAll_RemovedRoom", (roomId: string) => {
        console.log("receive removed room");
        dispatch(removeRoom(roomId));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_UpdatedPlayer", (updatedPlayer: IPlayerResponse) => {
        console.log("receive updated player");
        dispatch(updatePlayerInPlayers(updatedPlayer));
    });

    useClientMethod(signalRConnection, "ReceiveOwn_Readiness", (readiness: boolean) => {
        console.log("receive readiness");
        dispatch(setReadiness(readiness));
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

    useClientMethod(signalRConnection, "ReceiveOwn_NotEnoughMoneyNotification", (notification: INotificationResponse) => {
        console.log("receive not enough money notification");
        dispatch(setNewNotification(notification));

        setTimeout(() => {
            dispatch(removeNotification(notification.id))
        }, 5000);
    });

    useClientMethod(signalRConnection, "ReceiveGroup_NewRoomName", (newRoomName: string) => {
        console.log("receive new room name");
        dispatch(updateRoomName(newRoomName));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_NewCard", (card: ICardResponse) => {
        console.log(card);
        // TODO: finish
    });

    useClientMethod(signalRConnection, "ReceiveOwn_Card", (card: ICardResponse) => {
        console.log("own card", card);
        dispatch(setNewCard(card));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_UpdatedBank", (bankValue: number) => {
        console.log("receive updated bank");
        dispatch(updateBankValue(bankValue));
    });

    useClientMethod(signalRConnection, "ReceiveOwn_Move", () => {
        console.log("receive move");
        dispatch(setMove(true));

        startTimer
            .invoke(player.roomId, player.id)
            .catch(err => console.error(err.toString()));
    });

    useClientMethod(signalRConnection, "ReceiveMoveEnd", () => {
        console.log("receive move end");
        dispatch(setMove(false));
    })

    useClientMethod(signalRConnection, "ReceiveGroup_StartGameErrorNotification", (notification: string) => {
        // TODO: finish
    });

    useClientMethod(signalRConnection, "ReceiveOwn_UpdatedGameMoney", (moneyValue) => {
        console.log("receive updated game money");
        dispatch(setGameMoney(moneyValue));
    });

    useClientMethod(signalRConnection, "ReceiveGroup_StartGame", () => {
        console.log("receive start game");
        dispatch(setGameStarted(true));
    });

    useClientMethod(signalRConnection, "ReceiveOwn_UpdatedTimer", (seconds: number) => {
        console.log("receive updated timer");
        dispatch(setTimer(seconds));
    });

    useClientMethod(signalRConnection, "ReceiveOwn_InGame", (inGame: boolean) => {
        console.log("receive in game");
        dispatch(setInGame(true));
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