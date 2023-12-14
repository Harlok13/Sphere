// @ts-ignore
import style from "../RoomItem/RoomItem.module.css";  // inherited styles

export const RoomItemHead = (props) => {
    return (
        <li className={style.item}>
            <div className={style.body}>
                <img className={style.img}/>
                <div className={style.roomName}>
                    Room name
                    <span className={`${style.sort} material-icons-outlined`}>
                        {props.orderBy === "desc" ? "expand_less" : props.orderBy === "asc" ? "expand_more" : "minimize"}
                    </span>
                </div>
                <div className={style.roomSize}>
                    Players
                    <span className={`${style.sort} material-icons-outlined`}>
                        {props.orderBy === "desc" ? "expand_less" : props.orderBy === "asc" ? "expand_more" : "minimize"}
                    </span>
                </div>
                <div className={style.startBid}>
                    Start bid
                    <span className={`${style.sort} material-icons-outlined`}>
                        {props.orderBy === "desc" ? "expand_less" : props.orderBy === "asc" ? "expand_more" : "minimize"}
                    </span>
                </div>
                <div className={style.bid}>
                    Bid
                    <span className={`${style.sort} material-icons-outlined`}>
                        {props.orderBy === "desc" ? "expand_less" : props.orderBy === "asc" ? "expand_more" : "minimize"}
                    </span>
                </div>
                <div className={style.status}>
                    Status
                    <span className={`${style.sort} material-icons-outlined`}>
                        {props.orderBy === "desc" ? "expand_less" : props.orderBy === "asc" ? "expand_more" : "minimize"}
                    </span>
                </div>
            </div>
        </li>
    )
}