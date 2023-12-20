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
import {v4} from "uuid";
import {usePlayerInfoSelector} from "../../../BL/slices/player-info/use-player-info-selector";
import {
    useGame21PlayersSelector,
    useInRoomSelector, useRoomDataSelector
} from "../../../BL/slices/game21/use-game21-selector";

export const GlobalAside = () => {
    const players = useGame21PlayersSelector();
    const inRoom = useInRoomSelector();
    const playerInfo = usePlayerInfoSelector();
    const roomData = useRoomDataSelector();

    return (
        <Aside>
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
            <History>
                <HistoryHead/>
                <HistoryBody>
                    <UserHistoryMsg gameResult="Win">
                        <Game21HistoryMsg gameResult={"Win"} userName={"Harlok"} opponentName={"Bot"} score={"21:17"}/>
                    </UserHistoryMsg>
                </HistoryBody>
                <HistoryShowMore/>
            </History>
            {inRoom
                ? (<Participants>
                    <ParticipantsTitle roomName={roomData.roomName}/>
                    <ParticipantsList>
                        {players.length
                            ? players.map(p => (<Participant key={v4()} playerData={p}/>)) : null}
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