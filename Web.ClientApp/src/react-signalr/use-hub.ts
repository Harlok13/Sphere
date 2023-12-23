import {useEffect, useState} from "react";
import {HubConnection, HubConnectionState} from "@microsoft/signalr";

export const useHub = (hubConnection?: HubConnection) => {
    const [hubConnectionState, setHubConnectionState] = useState<HubConnectionState>(hubConnection?.state ?? HubConnectionState.Disconnected);
    const [error, setError] = useState();

    useEffect(() => {
        setError(undefined);

        if (!hubConnection) {
            setHubConnectionState(HubConnectionState.Disconnected);
            return;
        }

        if(hubConnection.state !== hubConnectionState) {
            setHubConnectionState(hubConnection.state);
        }

        let isMounted = true;
        const onStateUpdatedCallback = () => {
            if(isMounted) {
                setHubConnectionState(hubConnection?.state);
            }
        }
        hubConnection.onclose(onStateUpdatedCallback);
        hubConnection.onreconnected(onStateUpdatedCallback);
        hubConnection.onreconnecting(onStateUpdatedCallback);

        if (hubConnection.state === HubConnectionState.Disconnected) {
            const startPromise = hubConnection
                .start()
                .then(onStateUpdatedCallback)
                .catch(reason => setError(reason));
            onStateUpdatedCallback();

            return () => {
                startPromise.then(() => {
                    hubConnection.stop();
                });
                isMounted = false;
            };
        }

        return () => {
            hubConnection.stop();
        };
    }, [hubConnection]);

    return { hubConnectionState, error };
}