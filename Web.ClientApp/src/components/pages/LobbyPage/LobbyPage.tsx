import {ContentContainer} from "components/shared/components/ContentContainer/ContentContainer";
import {GlobalAside} from "components/layout/GlobalAside/GlobalAside";
import {Center} from "components/shared/pages/main-page/Center/Center";
import {GlobalHead} from "components/layout/GlobalHead/GlobalHead";
import {LobbyMain} from "components/pages/LobbyPage/center/LobbyMain/LobbyMain";
import {LobbyBottom} from "components/pages/LobbyPage/center/LobbyBottom/LobbyBottom";
import {GlobalRightSide} from "components/layout/GlobalRightSide/GlobalRightSide";
import {useLobbyPage} from "hooks/lobby/lobby-page/use-lobby-page";
import {Modal} from "components/shared/components/Modal/Modal";
import {useState} from "react";

export const LobbyPage = () => {
    useLobbyPage();

    const [modalInfoIsOpen, setModalInfoIsOpen] = useState(false);

    return (
        <ContentContainer>
            <GlobalAside/>
            <Center>
                {/*<button onClick={() => setModalInfoIsOpen(true)}>*/}
                {/*    show*/}
                {/*</button>*/}
                {/*    <Modal isOpen={modalInfoIsOpen} onClose={() => setModalInfoIsOpen(false)} timeout={350}>*/}
                {/*        <h2>Modal info</h2>*/}
                {/*        <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Accusantium asperiores cum dolore, eaque earum hic magni perferendis, quisquam ratione repellat suscipit unde, vero voluptatem. Accusamus aperiam fugit hic inventore iure!</p>*/}
                {/*    </Modal>*/}
                <GlobalHead/>
                <LobbyMain/>
                <LobbyBottom/>
            </Center>
            <GlobalRightSide/>
        </ContentContainer>
    )
}
