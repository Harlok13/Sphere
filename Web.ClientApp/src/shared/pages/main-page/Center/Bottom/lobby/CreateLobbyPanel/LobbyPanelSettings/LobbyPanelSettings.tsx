import style from "./LobbyPanelSettings.module.css";
import {FC} from "react";
import {LobbyPanelHandlers} from "../../../../../../../../BL/hooks/lobby/configure-room/use-configure-room";
import {NewRoomConfig} from "../../../../../../../../BL/slices/lobby/lobby.slice";
import {usePlayerInfoSelector} from "../../../../../../../../BL/slices/player-info/use-player-info-selector";
import {RoomSize} from "../../../../../../../../constants/configure-room-constants";

interface ILobbyPanelSettingsProps {
    newRoomData: NewRoomConfig;
    handlers: LobbyPanelHandlers;
}

export const LobbyPanelSettings: FC<ILobbyPanelSettingsProps> = ({newRoomData, handlers}) => {
    const {
        minBidHandler, maxBidHandler, startBidHandler,
        roomSizeHandler, roomNameHandler, createRoomHandler,
        lowBidHandler, mediumBidHandler, highBidHandler
    } = handlers;

    const playerInfo = usePlayerInfoSelector();  // TODO: hard code

    const {roomSize, startBid, minBid, maxBid, roomName, lowerBound, upperBound} = newRoomData;

    return (
        <form className={style.settings}>
            <div className={style.roomNameBlock}>
                <div className={playerInfo.money < lowerBound ? `${style.warning}`: ""}>{lowerBound} / {upperBound}</div>
                <label className={style.roomLabel} htmlFor="roomName">Room name:</label>
                <input className={style.roomNameInput} maxLength={20} required onChange={roomNameHandler} value={roomName} type="text"/>
            </div>

            <div className={style.body}>
                <div className={style.rangesBlock}>
                    <div className={style.rangeItem}>
                        <label htmlFor="roomSize">Room size:</label>
                        <input className={style.input} id="roomSize" onChange={roomSizeHandler} value={roomSize} type="range" min={RoomSize.MIN_SIZE} max={RoomSize.MAX_SIZE} step={RoomSize.STEP}/>
                        {/*<span className={style.value}>{roomSize}</span>*/}
                        <input onFocus={(e) => e.target.select()} className={style.value} onChange={roomSizeHandler} type="number" value={roomSize} max={RoomSize.MAX_SIZE} min={RoomSize.MIN_SIZE} step={RoomSize.STEP}/>
                    </div>

                    <div className={style.rangeItem}>
                        <label htmlFor="startBid">Start bid:</label>
                        <input className={style.input} id="startBid" onChange={startBidHandler} value={startBid} type="range" min="10" max="1000" step="10"/>
                        <span className={style.value}>{startBid}$</span>
                    </div>

                    <div className={style.rangeItem}>
                        <label htmlFor="minBid">Min bid:</label>
                        <input className={style.input} onChange={minBidHandler} value={minBid} id="minBid" type="range" min="5" max="200" step="5"/>
                        <span className={style.value}>{minBid}$</span>
                    </div>

                    <div className={style.rangeItem}>
                        <label htmlFor="maxBid">MaxBid:</label>
                        <input className={style.input} onChange={maxBidHandler} value={maxBid} id="maxBid" type="range" min="10" max="400" step="10"/>
                        <span className={style.value}>{maxBid}$</span>
                    </div>

                </div>

                <div className={style.buttons}>
                    <button onClick={lowBidHandler} className={`${style.button} ${style.low}`}></button>
                    <button onClick={mediumBidHandler} className={`${style.button} ${style.medium}`}></button>
                    <button onClick={highBidHandler} className={`${style.button} ${style.high}`}></button>
                </div>
            </div>

            <button className={style.createRoom} onClick={createRoomHandler}>Create room</button>
        </form>
    )
}
