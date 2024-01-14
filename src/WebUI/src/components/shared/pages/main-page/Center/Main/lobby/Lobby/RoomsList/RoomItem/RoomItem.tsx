import style from "./RoomItem.module.css";
import React, {FC} from "react";
import {JoinToRoomHandler} from "hooks/lobby/rooms-list/use-rooms-list";
import {Room} from "shared/contracts/room-in-lobby-dto";
// TODO: change the color to red if size is max

interface ILobbyItemProps {
    roomData: Room;
    joinToRoomHandler: JoinToRoomHandler;
}

export const RoomItem: FC<ILobbyItemProps> = ({roomData, joinToRoomHandler}) => {
    return (
        <li className={style.item}>
            <div className={style.body}>
                <img className={style.img} src={roomData.avatarUrl} alt="ava"/>
                <div className={style.roomName}>{roomData.roomName}</div>
                <div className={style.roomSize}><span className={style.value}>{roomData.playersInRoom}/{roomData.roomSize}</span></div>
                <div className={style.startBid}><span className={style.value}>{roomData.startBid}$</span></div>
                <div className={style.bid}><span className={style.value}>{roomData.minBid}/{roomData.maxBid}$</span></div>
                <div className={style.status}>{roomData.status}</div>
            </div>
            {/*todo rename class*/}
            <button className={style.join}>Info</button>
            {roomData.playersInRoom < roomData.roomSize
                ? (<button onClick={(e) => joinToRoomHandler(e, roomData)} className={style.join}>Join</button>)
                : null}
        </li>
    )
}

