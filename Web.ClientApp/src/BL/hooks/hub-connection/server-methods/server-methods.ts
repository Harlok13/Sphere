import {useHubMethod} from "react-use-signalr";
import {signalRConnection} from "../../../../App";

export const useCreateRoomHub = () =>
    useHubMethod(signalRConnection, "CreateRoom");

export const useJoinToRoomHub = () =>
    useHubMethod(signalRConnection, "JoinToRoom");

export const useRemoveFromRoomHub = () =>
    useHubMethod(signalRConnection, "RemoveFromRoom");

export const useToggleReadinessHub = () =>
    useHubMethod(signalRConnection, "ToggleReadiness");

export const useSelectStartGameMoneyHub = () =>
    useHubMethod(signalRConnection, "SelectStartGameMoney");

export const useStartGameHub = () =>
    useHubMethod(signalRConnection, "StartGame");

export const useStartTimerHub = () =>
    useHubMethod(signalRConnection, "StartTimer");

export const useStopTimerHub = () =>
    useHubMethod(signalRConnection, "StopTimer");