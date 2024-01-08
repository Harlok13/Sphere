import {GlobalAside} from "components/layout/GlobalAside/GlobalAside";
import {Center} from "components/shared/pages/main-page/Center/Center";
import {ContentContainer} from "components/shared/components/ContentContainer/ContentContainer";
import {GlobalHead} from "components/layout/GlobalHead/GlobalHead";
import {Main} from "components/shared/pages/main-page/Center/Main/Main";
import {Bottom} from "components/shared/pages/main-page/Center/Bottom/Bottom";
import {GlobalRightSide} from "components/layout/GlobalRightSide/GlobalRightSide";

const ProfilePage = () => {
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

export default ProfilePage;