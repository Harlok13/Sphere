import {ContentContainer} from "../../../shared/pages/main-page/ContentContainer/ContentContainer";
import {Center} from "../../../shared/pages/main-page/Center/Center";
import {GlobalAside} from "../../layout/GlobalAside/GlobalAside";
import {GlobalHead} from "../../layout/GlobalHead/GlobalHead";
import {GlobalRightSide} from "../../layout/GlobalRightSide/GlobalRightSide";
import {useLobbyInitData} from "../../../BL/hooks/init-data/use-lobby-init-data";
import {LobbyBottom} from "./center/LobbyBottom/LobbyBottom";
import {LobbyMain} from "./center/LobbyMain/LobbyMain";
import {useEffect} from "react";
import {useDispatch} from "react-redux";
import {setShowModal} from "../../../BL/slices/money/money.slice";

export const LobbyPage = () => {
    useLobbyInitData();
    const dispatch = useDispatch();

    useEffect(() => {
        return () => {
            dispatch(setShowModal(false));
        }
    }, []);

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