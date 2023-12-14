import {GlobalAside} from "../../layout/GlobalAside/GlobalAside";
import {Center} from "../../../shared/pages/main-page/Center/Center";
import {GlobalHead} from "../../layout/GlobalHead/GlobalHead";
import {Main} from "../../../shared/pages/main-page/Center/Main/Main";
import {Bottom} from "../../../shared/pages/main-page/Center/Bottom/Bottom";
import {GlobalRightSide} from "../../layout/GlobalRightSide/GlobalRightSide";
import {ContentContainer} from "../../../shared/pages/main-page/ContentContainer/ContentContainer";

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