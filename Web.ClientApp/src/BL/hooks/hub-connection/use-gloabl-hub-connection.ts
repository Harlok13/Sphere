import {useDispatch, useSelector} from "react-redux";
import {addNewRoom, removeRoom, updateRoom} from "../../slices/lobby.slice";
import {
    removePlayerFromPlayers,
    setNewPlayer,
    updatePlayerInPlayers,
    updatePlayersList,
    // setNewLeader
} from "../../slices/game21.slice";
import {useClientMethod} from "react-use-signalr";
import {signalRConnection} from "../../../App";
// import {useSendOwnDataHub, useSetNewLeaderHub} from "./server-methods/server-methods";
import {initPlayerData, setIsLeader} from "../../slices/player.slice";

export const useGlobalHubConnection = () => {
    const dispatch = useDispatch();
    // const sendOwnData = useSendOwnDataHub();
    // const setNewLeader = useSetNewLeaderHub();
    // @ts-ignore
    const game21 = useSelector(state => state.game21);
    // @ts-ignore
    const player = useSelector(state => state.playerId);

    useClientMethod(signalRConnection, "ReceiveMessage", (msg) => {
        console.log(msg);
    });

    useClientMethod(signalRConnection, "ReceiveNewPlayer", async (newPlayer) => {
        console.log(newPlayer, "new player");
        dispatch(setNewPlayer(newPlayer));
        // await sendOwnData.invoke(target, game21.player)
        //     .catch(err => console.error(err));
        console.log("set new player");
    });

    useClientMethod(signalRConnection, "ReceiveNewRoom", (newLobbyData) => {
        dispatch(addNewRoom(newLobbyData));
        console.log("add new room");
    });

    useClientMethod(signalRConnection, "ReceiveOwnPlayerData", (playerData, roomData, playersList) => {
        console.log("receive own player data");
        dispatch(initPlayerData(playerData));
        console.log(playersList, "players list");
        dispatch(updatePlayersList(playersList));

        // TODO: set room data
    });

    useClientMethod(signalRConnection, "ReceiveUpdatedPlayersList", (playersList) => {
        console.log("receive updated players list");
        dispatch(updatePlayersList(playersList));
    });

    useClientMethod(signalRConnection, "Test", (message) => {
        console.log(message);
    });

    // useClientMethod(signalRConnection, "NotifyRemoveFromRoom", async (removedPlayerId, msg) => {
    //     console.log(msg);
    //     // console.log(currentPlayers)
    //     dispatch(updatePlayersList(removedPlayerId));
    //     // debugger
    //     if(game21.players.length && !game21.players.some(p => p.isLeader === true)){
    //         const newLeader = game21.players[0];
    //         await setNewLeader.invoke(newLeader.roomGuid, newLeader.playerId)
    //             .catch(err => console.error(err));
    //     }
    //     console.log("confirm remove");
    // });

    useClientMethod(signalRConnection, "ReceiveRemovedPlayer", (removedPlayerId) => {
       dispatch(removePlayerFromPlayers(removedPlayerId));
    });

    useClientMethod(signalRConnection, "ReceiveChangedRoom", (changedRoom) => {
        console.log(changedRoom);
        dispatch(updateRoom(changedRoom));
        console.log("room updated");
    });

    // useClientMethod(signalRConnection, "ReceiveOwnData", (playerData) => {
    //     dispatch(setPlayerData(playerData));
    //     console.log("set player data");
    // });

    useClientMethod(signalRConnection, "ReceiveRemovedRoom", (roomGuid) => {
        dispatch(removeRoom(roomGuid));
    });

    // useClientMethod(signalRConnection, "ReceiveOpponentData", (opponentData) => {
    //     console.log(opponentData);
    //     dispatch(setNewPlayer(opponentData));  // TODO: fix
    // });

    useClientMethod(signalRConnection, "ReceiveNewLeader", (newLeaderData) => {
        console.log("set new leader");
        if (player.playerId === newLeaderData.playerId){
            dispatch(setIsLeader());
        }
        // dispatch(setNewLeader(newLeaderData));
        dispatch(updatePlayerInPlayers(newLeaderData));
    });

}
