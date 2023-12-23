import {ContentContainer} from "../../shared/pages/main-page/ContentContainer/ContentContainer";
import {GlobalAside} from "../../components/layout/GlobalAside/GlobalAside";
import {Main} from "../../shared/pages/main-page/Center/Main/Main";
import {GlobalHead} from "../../components/layout/GlobalHead/GlobalHead";
import {Center} from "../../shared/pages/main-page/Center/Center";
import {GlobalRightSide} from "../../components/layout/GlobalRightSide/GlobalRightSide";
import {Bottom} from "../../shared/pages/main-page/Center/Bottom/Bottom";

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