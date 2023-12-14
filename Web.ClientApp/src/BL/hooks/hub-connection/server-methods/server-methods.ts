import {useHubMethod} from "react-use-signalr";
import {signalRConnection} from "../../../../App";

export const useCreateRoomHub = () => {
    return useHubMethod(signalRConnection, "CreateRoom");
}

export const useJoinToRoomHub = () => {
    return useHubMethod(signalRConnection, "JoinToRoom");
}

export const useRemoveFromRoomHub = () => {
    return useHubMethod(signalRConnection, "RemoveFromRoom");
}

export const useTestMethodHub = () => {
    return useHubMethod(signalRConnection, "GetTest");
}

// export const useSendOwnDataHub = () => {
//     return useHubMethod(signalRConnection, "SendOwnData");
// }

// export const useSetNewLeaderHub = () => {
//     return useHubMethod(signalRConnection, "SetNewLeader");
// }