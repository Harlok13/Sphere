import {PropsWithChildren, useEffect} from "react";
import {useRemoveFromRoomHub} from "../../../../BL/hooks/hub-connection/server-methods/server-methods";
import {useSelector} from "react-redux";

export const BeforeUnload = ({children}: PropsWithChildren) => {
    // @ts-ignore
    const game21 = useSelector(state => state.game21);
    const removeFromRoom = useRemoveFromRoomHub();

    const componentDidMount = () => {
        window.addEventListener("beforeunload", handleBeforeUnload);
    }
    useEffect(() => {
        componentDidMount();

        return () => {
            componentWillUnmount();
        }
    }, []);

    const componentWillUnmount = () => {
        window.removeEventListener("beforeunload", handleBeforeUnload);
    }

    const handleBeforeUnload = (e) => {
        removeFromRoom.invoke(game21.guid, game21.player.playerId)
        e.returnValue = "Are you sure you want to exit?";
    }

    return <>{children}</>;
}