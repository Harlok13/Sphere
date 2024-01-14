import {ChatMessage, Message} from "components/shared/components/ChatMessage/ChatMessage";
import {RightSide} from "components/shared/pages/main-page/RightSide/RightSide";
import {RoomChat} from "components/shared/pages/main-page/RightSide/RoomChat/RoomChat";
import {RoomChatHead} from "components/shared/pages/main-page/RightSide/RoomChat/RoomChatHead/RoomChatHead";
import {MessageWindow} from "components/shared/pages/main-page/RightSide/RoomChat/MessageWindow/MessageWindow";
import {TextArea} from "components/shared/pages/main-page/RightSide/RoomChat/TextArea/TextArea";

export const GlobalRightSide = () => {
    const msgData : Message = {
        currentTime: "21:32",
        playerName: "Lara",
        msgText: "wow"
    }

    const props = {

    }
    return (
        <RightSide>
            <RoomChat>
                <RoomChatHead/>
                <MessageWindow>
                    <ChatMessage msgData={msgData}/>
                </MessageWindow>
                <TextArea props={props}/>
            </RoomChat>
        </RightSide>
    )
}