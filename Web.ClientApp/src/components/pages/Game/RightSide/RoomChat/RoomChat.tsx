// @ts-ignore
import style from "./style.module.css";
import {useEffect, useRef, useState} from "react";
import * as signalR from "@microsoft/signalr";
import {ChatMessage} from "../../../../../shared/components/ChatMessage/ChatMessage";
import {v4} from "uuid";

const RoomChat = () => {
    const [hubConnection, setHubConnection] = useState(null);
    const [message, setMessage] = useState("");
    const sendBtnRef = useRef();

    const [chatMessages, setChatMessages] = useState([]);

    const user = JSON.parse(localStorage.getItem("user"));

    const currentTime = () =>{  // TODO: duplicate, relocate to service?
        const date = new Date();
        return `${String(date.getHours()).padStart(2, "0")}:${String(date.getMinutes()).padStart(2, "0")}`;
    }

    useEffect(() => {
        const startHubConnection = async () => {
            const connection = new signalR.HubConnectionBuilder()
                .withUrl("https://localhost:7170/chat")
                .build();

            connection.on("ReceiveMessage", (user, message) => {
                setChatMessages(prev => [...prev, {currentTime: currentTime(), userName: user, text: message}])
            });

            try{
                await connection.start();
                setHubConnection(connection);
                // @ts-ignore
                sendBtnRef.current.disabled = false;
            } catch (err) {
                console.error(err.toString());
            }
        };

        startHubConnection();

        return () => {
            if (hubConnection){
                hubConnection.off("ReceiveMessage");
                hubConnection.stop();
            }
        };
    }, []);

    const sendMessage = () => {
        if (!hubConnection) return;

        hubConnection
            .invoke("SendMessage", user.userName, message)
            .catch((err) => console.error(err.toString()));

        setMessage("");
    };

    const messageHandler = (e) => {
        setMessage(e.target.value);
    }

    return (
        <div className={style.chat}>
            <div className={style.head}>Room chat</div>
            <div className={style.messageWindow}>
                <div className={style.message}>
                    <div className={style.leftSide}>
                        <img className={style.avatar} src="/img/avatars/ava.jpg" alt="ava"/>
                        <div className={style.time}>21:35</div>
                    </div>
                    <div className={style.body}>
                        <div className={style.messageHead}>
                            <div className={style.username}>Lara</div>
                            <span className={`${style.menu} material-icons-outlined`}>
                                more_vert
                            </span>
                        </div>
                        <div className={style.text}>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ab distinctio expedita officiis temporibus. Ad adipisci alias commodi cum dicta eius fuga harum laudantium minus nemo omnis provident, quia repellat ut!</div>
                    </div>
                </div>
                <div className={`${style.message} ${style.message_reverse}`}>
                    <div className={style.leftSide}>
                        <img className={style.avatar} src="/img/avatars/ava.jpg" alt="ava"/>
                        <div className={style.time}>21:35</div>
                    </div>
                    <div className={style.body}>
                        <div className={style.messageHead}>
                            <div className={style.username}>Harlok</div>
                            <span className={`${style.menu} material-icons-outlined`}>
                                more_vert
                            </span>
                        </div>
                        <div className={style.text}>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ab distinctio expedita officiis temporibus. Ad adipisci alias commodi cum dicta eius fuga harum laudantium minus nemo omnis provident, quia repellat ut!</div>
                    </div>
                </div>
                {chatMessages.length
                    ? chatMessages.map(msgData => (<ChatMessage key={v4()} msgData={msgData}/>))
                    : null
                }
            </div>
            <div className={style.textArea}>
                <textarea value={message} onChange={messageHandler} className={style.input} name="" ></textarea>
                <span onClick={sendMessage} ref={sendBtnRef} className={`${style.iconSend} material-icons-outlined`}>
                    send
                </span>
            </div>
        </div>
    )
}

export default RoomChat;
