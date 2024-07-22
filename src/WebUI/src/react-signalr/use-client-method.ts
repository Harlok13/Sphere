import {useEffect} from "react";
import {HubConnection} from "@microsoft/signalr";

export const useClientMethod = (hubConnection: HubConnection | undefined, methodName: string, method: (...args: any[]) => void) => {
    useEffect(() => {
        if(!hubConnection) {
            return;
        }

        hubConnection.on(methodName, method);

        return () => {
            hubConnection.off(methodName, method);
        }

    }, [hubConnection, method, methodName]);
}