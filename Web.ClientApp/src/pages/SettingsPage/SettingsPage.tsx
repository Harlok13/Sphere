import {GlobalAside} from "../../components/layout/GlobalAside/GlobalAside";
import {Center} from "../../shared/pages/main-page/Center/Center";
import {GlobalHead} from "../../components/layout/GlobalHead/GlobalHead";
import {Main} from "../../shared/pages/main-page/Center/Main/Main";
import {Bottom} from "../../shared/pages/main-page/Center/Bottom/Bottom";
import {GlobalRightSide} from "../../components/layout/GlobalRightSide/GlobalRightSide";
import {ContentContainer} from "../../shared/pages/main-page/ContentContainer/ContentContainer";

export const SettingsPage = () => {
    return (
        <ContentContainer>
            <GlobalAside/>
            <Center>
                <GlobalHead/>
                <Main>
                    control buttons settings
                </Main>
                {/*<Bottom>*/}

                {/*</Bottom>*/}
            </Center>
            <GlobalRightSide/>
        </ContentContainer>
    )
}