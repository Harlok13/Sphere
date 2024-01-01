import {ContentContainer} from "shared/pages/main-page/ContentContainer/ContentContainer";
import {Center} from "shared/pages/main-page/Center/Center";
import {GlobalAside} from "components/layout/GlobalAside/GlobalAside";
import {GlobalHead} from "components/layout/GlobalHead/GlobalHead";
import {GlobalRightSide} from "components/layout/GlobalRightSide/GlobalRightSide";
import {LobbyBottom} from "pages/LobbyPage/center/LobbyBottom/LobbyBottom";
import {LobbyMain} from "pages/LobbyPage/center/LobbyMain/LobbyMain";
import {useEffect} from "react";
import {useDispatch} from "react-redux";
import {setSelectStartMoneyModal} from "BL/slices/modals/modals.slice";

export const LobbyPage = () => {
    // useLobbyInitData();
    const dispatch = useDispatch();

    useEffect(() => {
        return () => {
            dispatch(setSelectStartMoneyModal(false));
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
