import {UserInfoAvatar} from "components/shared/pages/main-page/Aside/UserInfo/UserInfoAvatar/UserInfoAvatar";
import {UserInfoBody} from "components/shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoBody";
import {UserInfoHead} from "components/shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoHead/UserInfoHead";
import {UserName} from "components/shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoHead/UserName/UserName";
import {Logout} from "components/shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoHead/Logout/Logout";
import {UserInfoItems} from "components/shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoItems/UserInfoItems";
import {Money} from "components/shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoItems/Money/Money";
import {Level} from "components/shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoItems/Level/Level";
import {UserInfo} from "components/shared/pages/main-page/Aside/UserInfo/UserInfo";
import React from "react";
import {usePlayerInfoSelector} from "store/player-info/use-player-info-selector";

export const GlobalUserInfo = () => {
    const playerInfo = usePlayerInfoSelector();

    return (
        <UserInfo>
            <UserInfoAvatar avatarUrl={playerInfo.avatarUrl}/>
            <UserInfoBody>
                <UserInfoHead>
                    <UserName name={playerInfo.playerName}/>
                    <Logout/>
                </UserInfoHead>
                <UserInfoItems>
                    <Money money={playerInfo.money}/>
                    <Level playerInfo={playerInfo}/>
                </UserInfoItems>
            </UserInfoBody>
        </UserInfo>
    )
}