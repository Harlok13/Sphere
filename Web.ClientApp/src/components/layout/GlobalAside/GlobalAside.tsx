import {UserInfo} from "../../../shared/pages/main-page/Aside/UserInfo/UserInfo";
import {UserInfoAvatar} from "../../../shared/pages/main-page/Aside/UserInfo/UserInfoAvatar/UserInfoAvatar";
import {UserInfoBody} from "../../../shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoBody";
import {UserInfoHead} from "../../../shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoHead/UserInfoHead";
import {UserName} from "../../../shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoHead/UserName/UserName";
import {Logout} from "../../../shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoHead/Logout/Logout";
import {UserInfoItems} from "../../../shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoItems/UserInfoItems";
import {Money} from "../../../shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoItems/Money/Money";
import {Level} from "../../../shared/pages/main-page/Aside/UserInfo/UserInfoBody/UserInfoItems/Level/Level";
import {History} from "../../../shared/pages/main-page/Aside/History/History";
import {HistoryHead} from "../../../shared/pages/main-page/Aside/History/HistoryHead/HistoryHead";
import {HistoryBody} from "../../../shared/pages/main-page/Aside/History/HistoryBody/HistoryBody";
import {UserHistoryMsg} from "../../../shared/pages/main-page/Aside/History/HistoryBody/UserHistoryMsg/UserHistoryMsg";
import {
    Game21HistoryMsg
} from "../../../shared/pages/main-page/Aside/History/HistoryBody/UserHistoryMsg/Game21HistoryMsg/Game21HistoryMsg";
import {HistoryShowMore} from "../../../shared/pages/main-page/Aside/History/HistoryShowMore/HistoryShowMore";
import {Participants} from "../../../shared/pages/main-page/Aside/Participants/Participants";
import {ParticipantsTitle} from "../../../shared/pages/main-page/Aside/Participants/ParticipantsTitle/ParticipantsTitle";
import {ParticipantsList} from "../../../shared/pages/main-page/Aside/Participants/ParticipantsList/ParticipantsList";
import {Participant} from "../../../shared/pages/main-page/Aside/Participants/ParticipantsList/Participant/Participant";
import {RoomControlPanel} from "../../../shared/pages/main-page/Aside/Participants/RoomControlPanel/RoomControlPanel";
import {LeaveButton} from "../../../shared/pages/main-page/Aside/Participants/RoomControlPanel/LeaveButton/LeaveButton";
import {InviteButton} from "../../../shared/pages/main-page/Aside/Participants/RoomControlPanel/InviteButton/InviteButton";
import {Aside} from "../../../shared/pages/main-page/Aside/Aside";
import {useSelector} from "react-redux";
import {v4} from "uuid";

export const GlobalAside = () => {
    // @ts-ignore
    const game21 = useSelector(state => state.game21);
    // @ts-ignore
    const userInfo = useSelector(state => state.userInfo);

    return (
        <Aside>
            <UserInfo>
                <UserInfoAvatar props={userInfo}/>
                <UserInfoBody>
                    <UserInfoHead>
                        <UserName props={userInfo}/>
                        <Logout/>
                    </UserInfoHead>
                    <UserInfoItems>
                        <Money props={userInfo}/>
                        <Level props={userInfo}/>
                    </UserInfoItems>
                </UserInfoBody>
            </UserInfo>
            <History>
                <HistoryHead/>
                <HistoryBody>
                    <UserHistoryMsg gameResult="Win">
                        <Game21HistoryMsg gameResult={"Win"} userName={"Harlok"} opponentName={"Bot"} score={"21:17"}/>
                    </UserHistoryMsg>
                </HistoryBody>
                <HistoryShowMore/>
            </History>
            {game21.inRoom
                ? (<Participants>
                    <ParticipantsTitle roomName="Game 21"/>
                    <ParticipantsList>
                        {game21.players.length
                            ? game21.players.map(p => (<Participant key={v4()} isLeader={p.isLeader} playerName={p.playerName} readiness={p.readiness}/>)) : null}
                    </ParticipantsList>
                    <RoomControlPanel>
                        <LeaveButton/>
                        <InviteButton/>
                    </RoomControlPanel>
                </Participants>)
                : null}
        </Aside>
    )
}