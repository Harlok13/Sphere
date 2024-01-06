import {useDispatch} from "react-redux";
import {useEffect} from "react";
import {setSelectStartMoneyModal} from "store/modals/modals.slice";
import {ContentContainer} from "components/shared/pages/main-page/ContentContainer/ContentContainer";
import {GlobalAside} from "components/layout/GlobalAside/GlobalAside";
import {Center} from "components/shared/pages/main-page/Center/Center";
import {GlobalHead} from "components/layout/GlobalHead/GlobalHead";
import {LobbyMain} from "components/pages/LobbyPage/center/LobbyMain/LobbyMain";
import {LobbyBottom} from "components/pages/LobbyPage/center/LobbyBottom/LobbyBottom";
import {GlobalRightSide} from "components/layout/GlobalRightSide/GlobalRightSide";

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
