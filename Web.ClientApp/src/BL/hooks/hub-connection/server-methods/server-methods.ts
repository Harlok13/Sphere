import {signalRConnection} from "../../../../App";
import {useHubMethod} from "../../../../react-signalr/use-hub-method";
import {ISelectStartGameMoneyRequest} from "../../../../contracts/requests/select-start-game-money-request";
import {ICreateRoomRequest} from "../../../../contracts/requests/create-room-request";
import {IJoinToRoomRequest} from "../../../../contracts/requests/join-to-room-request";
import {IRemoveFromRoomRequest} from "../../../../contracts/requests/remove-from-room-request";
import {IToggleReadinessRequest} from "../../../../contracts/requests/toggle-readiness-request";
import {IStartGameRequest} from "../../../../contracts/requests/start-game-request";
import {IStartTimerRequest} from "../../../../contracts/requests/start-timer-request";
import {IStopTimerRequest} from "../../../../contracts/requests/stop-timer-request";


export const useCreateRoomHub = () =>
    useHubMethod<boolean, ICreateRoomRequest>(signalRConnection, "CreateRoom");

export const useJoinToRoomHub = () =>
    useHubMethod<boolean, IJoinToRoomRequest>(signalRConnection, "JoinToRoom");

export const useRemoveFromRoomHub = () =>
    useHubMethod<boolean, IRemoveFromRoomRequest>(signalRConnection, "RemoveFromRoom");

export const useToggleReadinessHub = () =>
    useHubMethod<boolean, IToggleReadinessRequest>(signalRConnection, "ToggleReadiness");

export const useSelectStartGameMoneyHub = () =>
    useHubMethod<boolean, ISelectStartGameMoneyRequest>(signalRConnection, "SelectStartGameMoney");

export const useStartGameHub = () =>
    useHubMethod<boolean, IStartGameRequest>(signalRConnection, "StartGame");

export const useStartTimerHub = () =>
    useHubMethod<boolean, IStartTimerRequest>(signalRConnection, "StartTimer");

export const useStopTimerHub = () =>
    useHubMethod<boolean, IStopTimerRequest>(signalRConnection, "StopTimer");