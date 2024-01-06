import {useConfigureRoom} from "hooks/lobby/configure-room/use-configure-room";
import {Bottom} from "components/shared/pages/main-page/Center/Bottom/Bottom";
import {
    CreateLobbyPanel
} from "components/shared/pages/main-page/Center/Bottom/lobby/CreateLobbyPanel/CreateLobbyPanel";
import {
    LobbyPanelSettings
} from "components/shared/pages/main-page/Center/Bottom/lobby/CreateLobbyPanel/LobbyPanelSettings/LobbyPanelSettings";
import {NotificationPanel} from "components/shared/pages/main-page/Center/Bottom/NotificationPanel/NotificationPanel";

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