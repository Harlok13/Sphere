import style from "./RoomItem.module.css";
import {MouseEventHandler} from "react";
import {Link} from "react-router-dom";
// TODO: change the color to red if size is max

type LobbyItemProps = {
    guid: string;
    imgUrl: string;
    roomName: string;
    roomSize: number;
    startBid: number;
    minBid: number;
    maxBid: number;
    status: string;
    playersInRoom: number;
    joinToRoomHandler: JoinToRoomHandler;
}

type JoinToRoomHandler = (e: React.MouseEvent<HTMLButtonElement>, guid: string) => Promise<void>;

// export const RoomItem = (lobbyData: LobbyItemProps) => {
export const RoomItem = ({props, joinToRoomHandler}) => {
    // console.log(lobbyData, "lobby")
    return (
        <li className={style.item}>
            <div className={style.body}>
                <img className={style.img} src={props.imgUrl} alt="ava"/>
                <div className={style.roomName}>{props.roomName}</div>
                <div className={style.roomSize}><span className={style.value}>{props.playersInRoom}/{props.roomSize}</span></div>
                <div className={style.startBid}><span className={style.value}>{props.startBid}$</span></div>
                <div className={style.bid}><span className={style.value}>{props.minBid}/{props.maxBid}$</span></div>
                <div className={style.status}>{props.status}</div>
            </div>
            {props.playersInRoom < props.roomSize
                ? (<button onClick={(e) => joinToRoomHandler(e, props.id)} className={style.join}>Join</button>)
                : null}
        </li>
    )
}
