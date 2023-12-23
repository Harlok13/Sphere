import {useCallback, useEffect, useRef, useState} from "react";
import {HubConnection} from "@microsoft/signalr";

type HubMethodState<TData = any> = {
    loading: boolean,
    data?: TData,
    error?: any
}
const initialState : HubMethodState = {
    loading: false
}

export function useHubMethod<TData, TArgs extends any>(hubConnection: HubConnection | undefined, methodName: string) {
    const [state, setState] = useState<HubMethodState<TData>>(initialState);
    const isMounted = useRef(true);

    const setStateIfMounted: typeof setState = useCallback((value) => {
        if(isMounted.current) {
            setState(value);
        }
    }, []);

    const invoke = useCallback(async (args: TArgs) => {
        setStateIfMounted(s => ({...s, loading: true}));

        try {
            if(hubConnection) {
                const data = await hubConnection.invoke<TData>(methodName, args);
                setStateIfMounted(s => ({...s, data: data, loading: false, error: undefined}));
                return data;
            }
            else {
                throw new Error('hubConnection is not defined');
            }
        }
        catch(e) {
            setStateIfMounted(s => ({...s, error: e, loading: false}));
        }
    }, [hubConnection, methodName]);

    useEffect(() => () => {isMounted.current = false}, []);

    return { invoke, ...state };
}
