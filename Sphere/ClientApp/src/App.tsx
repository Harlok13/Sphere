import './custom.scss';
import {Router} from "./routes/Router";
import Wrapper from "./shared/components/Wrapper/Wrapper";
import {useGlobalHubConnection} from "./BL/hooks/hub-connection/use-gloabl-hub-connection";
import {HubConnectionBuilder, LogLevel} from "@microsoft/signalr";
import {useHub} from "react-use-signalr";

export const signalRConnection = new HubConnectionBuilder()
    .withUrl("https://localhost:7170/global")
    .configureLogging(LogLevel.Information)
    .withAutomaticReconnect()
    .build();

export const App = () => {
    useGlobalHubConnection();

    const { hubConnectionState, error } = useHub(signalRConnection);

    return (
        <Wrapper>
            <Router/>
        </Wrapper>
    );
}
