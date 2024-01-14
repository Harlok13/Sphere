import {PropsWithChildren} from "react";
import {useGlobalHubConnection} from "hooks/hub-connection/use-gloabl-hub-connection";
import {useHub} from "react-signalr/use-hub";
import {HubConnectionBuilder, LogLevel} from "@microsoft/signalr";
import UserService from "services/user/user.service";

const URL = "http://localhost:5083/hubs/global"
// const URL = "https://localhost:7170/hubs/global"

export const signalRConnection = new HubConnectionBuilder()
    .withUrl(URL, {
        accessTokenFactory(): string | Promise<string> {
            return UserService.getUser().token;
        }
    })
    .configureLogging(LogLevel.Information)
    .withAutomaticReconnect()
    .build();

export const SignalrProvider = ({children}: PropsWithChildren) => {
    useGlobalHubConnection();
    useHub(signalRConnection)

    return <>{children}</>
}