import './custom.scss';
import {Router} from "./routes/Router";
import Wrapper from "./shared/components/Wrapper/Wrapper";
import {useGlobalHubConnection} from "./BL/hooks/hub-connection/use-gloabl-hub-connection";
import {HubConnectionBuilder, LogLevel} from "@microsoft/signalr";
import {useHub} from "react-use-signalr";
import {useDispatch, useSelector} from "react-redux";
import {useRemoveFromRoomHub} from "./BL/hooks/hub-connection/server-methods/server-methods";

export const signalRConnection = new HubConnectionBuilder()
    .withUrl("https://localhost:7170/hubs/global")
    .configureLogging(LogLevel.Information)
    .withAutomaticReconnect()
    .build();


// window.onbeforeunload = function () {
//     // return false;
//     return "progress may be lost, are you sure?";
// };


export const App = () => {
    useGlobalHubConnection();

    // @ts-ignore
    const game21 = useSelector(state => state.game21);
    const removeFromRoom = useRemoveFromRoomHub();

    const dispatcher = useDispatch();

    // const handleBeforeUnload = () => {
    //     removeFromRoom.invoke(game21.guid, game21.player.playerId)
    // }
    //
    // window.addEventListener("beforeunload", handleBeforeUnload);

    const {hubConnectionState, error} = useHub(signalRConnection);

    return (
        <Wrapper>
            <Router/>
        </Wrapper>
    );
}
