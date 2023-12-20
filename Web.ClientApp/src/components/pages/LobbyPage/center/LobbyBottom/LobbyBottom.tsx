import {
    CreateLobbyPanel
} from "../../../../../shared/pages/main-page/Center/Bottom/lobby/CreateLobbyPanel/CreateLobbyPanel";
import {
    LobbyPanelSettings
} from "../../../../../shared/pages/main-page/Center/Bottom/lobby/CreateLobbyPanel/LobbyPanelSettings/LobbyPanelSettings";
import {
    NotificationPanel
} from "../../../../../shared/pages/main-page/Center/Bottom/NotificationPanel/NotificationPanel";
import {Bottom} from "../../../../../shared/pages/main-page/Center/Bottom/Bottom";
import {useConfigureRoom} from "../../../../../BL/hooks/lobby/configure-room/use-configure-room";

export const LobbyBottom = () => {
    const {handlers, newRoomData} = useConfigureRoom();

    return (
        <Bottom>
            <CreateLobbyPanel>
                <LobbyPanelSettings handlers={handlers} newRoomData={newRoomData}/>
            </CreateLobbyPanel>
            <NotificationPanel/>
        </Bottom>
    )
}