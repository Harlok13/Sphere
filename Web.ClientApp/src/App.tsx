import 'index.scss';
import {Router} from "routes/Router";
import Wrapper from "shared/components/Wrapper/Wrapper";
import {useGlobalHubConnection} from "BL/hooks/hub-connection/use-gloabl-hub-connection";
import {HubConnectionBuilder, LogLevel} from "@microsoft/signalr";
import {useHub} from "react-use-signalr";
import {Notifications} from "shared/components/Notifications/Notifications";
import UserService from "./services/user/user.service";

export const signalRConnection = new HubConnectionBuilder()
    .withUrl("https://localhost:7170/hubs/global", {
        accessTokenFactory(): string | Promise<string> {
            return UserService.getUser().token;
        }
    })
    .configureLogging(LogLevel.Information)
    .withAutomaticReconnect()
    .build();


// window.onbeforeunload = function () {
//     // return false;
//     return "progress may be lost, are you sure?";
// };


export const App = () => {
    useGlobalHubConnection();

    // const handleBeforeUnload = () => {
    //     removeFromRoom.invoke(game21.guid, game21.player.playerId)
    // }
    //
    // window.addEventListener("beforeunload", handleBeforeUnload);

    const {hubConnectionState, error} = useHub(signalRConnection);

    return (
        <Notifications>
            <Wrapper>
                <Router/>
            </Wrapper>
        </Notifications>
    );
}
