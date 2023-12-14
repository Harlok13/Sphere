// @ts-ignore
import style from "../style.module.css";
import {UserHistory} from "./UserHistory/UserHistory";
import {v4} from "uuid";
import {useEffect, useRef} from "react";

const Items = ({props}) => {
    // const userHistoryRef = useRef(null);
    //
    // useEffect(() => {
    //
    // }, [props.userHistory])

    return (
        <div className={style.items}>
            {props.userHistory.length
                ? props.userHistory.map(historyData => (<UserHistory key={v4()} historyData={historyData}/>))
                : null
            }
        </div>
    )
}

export default Items;