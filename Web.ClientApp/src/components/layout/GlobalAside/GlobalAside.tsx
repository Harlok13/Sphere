import React from "react";
import {Aside} from "components/shared/pages/main-page/Aside/Aside";
import {GlobalHistory} from "components/layout/GlobalAside/GlobalHistory/GlobalHistory";
import {GlobalUserInfo} from "components/layout/GlobalAside/GlobalUserInfo/GlobalUserInfo";
import {GlobalParticipants} from "components/layout/GlobalAside/GlobalParticipants/GlobalParticipants";


export const GlobalAside = () => {
    return (
        <Aside>
            <GlobalUserInfo/>
            <GlobalHistory/>
            <GlobalParticipants/>
        </Aside>
    )
}