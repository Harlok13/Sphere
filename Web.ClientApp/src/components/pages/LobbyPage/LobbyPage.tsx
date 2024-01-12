import {ContentContainer} from "components/shared/components/ContentContainer/ContentContainer";
import {GlobalAside} from "components/layout/GlobalAside/GlobalAside";
import {Center} from "components/shared/pages/main-page/Center/Center";
import {GlobalHead} from "components/layout/GlobalHead/GlobalHead";
import {LobbyMain} from "components/pages/LobbyPage/center/LobbyMain/LobbyMain";
import {LobbyBottom} from "components/pages/LobbyPage/center/LobbyBottom/LobbyBottom";
import {GlobalRightSide} from "components/layout/GlobalRightSide/GlobalRightSide";
import {useLobbyPage} from "hooks/lobby/lobby-page/use-lobby-page";

export const LobbyPage = () => {
    useLobbyPage();

    return (
        <ContentContainer>
            <GlobalAside/>
            <Center>
                <GlobalHead/>
                <LobbyMain/>
                <LobbyBottom/>
            </Center>
            <GlobalRightSide/>
        </ContentContainer>
    )
}
