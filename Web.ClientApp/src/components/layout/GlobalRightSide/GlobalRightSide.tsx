import {RightSide} from "../../../shared/pages/main-page/RightSide/RightSide";
import {RoomChat} from "../../../shared/pages/main-page/RightSide/RoomChat/RoomChat";
import {RoomChatHead} from "../../../shared/pages/main-page/RightSide/RoomChat/RoomChatHead/RoomChatHead";
import {MessageWindow} from "../../../shared/pages/main-page/RightSide/RoomChat/MessageWindow/MessageWindow";
import {TextArea} from "../../../shared/pages/main-page/RightSide/RoomChat/TextArea/TextArea";
import {ChatMessage, Message} from "../../../shared/components/ChatMessage/ChatMessage";

export const GlobalRightSide = () => {
    const msgData : Message = {
        currentTime: "21:32",
        playerName: "Harlok",
        msgText: "some text"
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