import React from "react";
import {GlobalAside} from "components/layout/GlobalAside/GlobalAside";
import {Center} from "components/shared/pages/main-page/Center/Center";
import {ContentContainer} from "components/shared/components/ContentContainer/ContentContainer";
import {GlobalHead} from "components/layout/GlobalHead/GlobalHead";
import {Main} from "components/shared/pages/main-page/Center/Main/Main";
import {GlobalRightSide} from "components/layout/GlobalRightSide/GlobalRightSide";

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