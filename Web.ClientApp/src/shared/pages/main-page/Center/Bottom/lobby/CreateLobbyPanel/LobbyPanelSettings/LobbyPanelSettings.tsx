import style from "./LobbyPanel.module.css";

export const LobbyPanelSettings = ({roomSettings, handlers}) => {
    const {
        minBidHandler, maxBidHandler, startBidHandler,
        roomSizeHandler, roomNameHandler, createRoomHandler,
        lowBidHandler, mediumBidHandler, highBidHandler
    } = handlers;

    const {roomSize, startBid, minBid, maxBid, roomName} = roomSettings;

    return (
        <form className={style.settings}>
            <div className={style.roomNameBlock}>
                <label className={style.roomLabel} htmlFor="roomName">Room name:</label>
                <input className={style.roomNameInput} maxLength={20} required onChange={roomNameHandler} value={roomName} type="text"/>
            </div>

            <div className={style.body}>
                <div className={style.rangesBlock}>
                    <div className={style.rangeItem}>
                        <label htmlFor="roomSize">Room size:</label>
                        <input className={style.input} id="roomSize" onChange={roomSizeHandler} value={roomSize} type="range" min="2" max="4"/>
                        <span className={style.value}>{roomSize}</span>
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
