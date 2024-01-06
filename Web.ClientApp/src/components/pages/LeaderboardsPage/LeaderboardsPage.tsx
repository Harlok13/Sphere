import {ContentContainer} from "components/shared/pages/main-page/ContentContainer/ContentContainer";
import {GlobalAside} from "components/layout/GlobalAside/GlobalAside";
import {Center} from "components/shared/pages/main-page/Center/Center";
import {GlobalHead} from "components/layout/GlobalHead/GlobalHead";
import {Main} from "components/shared/pages/main-page/Center/Main/Main";
import {Bottom} from "components/shared/pages/main-page/Center/Bottom/Bottom";
import {GlobalRightSide} from "components/layout/GlobalRightSide/GlobalRightSide";

export const LeaderboardsPage = () => {
    return (
        <ContentContainer>
            <GlobalAside/>
            <Center>
                <GlobalHead/>
                <Main>
                </Main>
                <Bottom>

                </Bottom>
            </Center>
            <GlobalRightSide/>
        </ContentContainer>
    )
}