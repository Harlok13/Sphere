import {PropsWithChildren} from "react";
import {useGlobalHubConnection} from "hooks/hub-connection/use-gloabl-hub-connection";
import {useHub} from "react-signalr/use-hub";
import {HubConnectionBuilder, LogLevel} from "@microsoft/signalr";
import UserService from "services/user/user.service";

export const signalRConnection = new HubConnectionBuilder()
    .withUrl("https://localhost:7170/hubs/global", {
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