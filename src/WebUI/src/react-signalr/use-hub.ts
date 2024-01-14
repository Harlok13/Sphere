import {useEffect, useState} from "react";
import {HubConnection, HubConnectionState} from "@microsoft/signalr";
import {v4} from "uuid";
import {useDispatch} from "react-redux";
import {INotificationResponse} from "shared/contracts/notification-response";
import {setNewNotification} from "store/notifications/notifications.slice";

export const useHub = (hubConnection?: HubConnection) => {
    const [hubConnectionState, setHubConnectionState] = useState<HubConnectionState>(hubConnection?.state ?? HubConnectionState.Disconnected);
    const [error, setError] = useState();

    const dispatch = useDispatch();

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

                const notification: INotificationResponse = {
                    notificationId: v4(),
                    notificationText: "Connection restored."
                }
                dispatch(setNewNotification(notification));  // TODO: set timeout
            }
        }
        hubConnection.onclose(onStateUpdatedCallback);
        hubConnection.onreconnected(onStateUpdatedCallback);
        hubConnection.onreconnecting(onStateUpdatedCallback);

        if (hubConnection.state === HubConnectionState.Disconnected) {
            const notification: INotificationResponse = {
                notificationId: v4(),
                notificationText: "Connection lost. Try reloading the page."
            }
            dispatch(setNewNotification(notification));  // TODO: set timeout
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
