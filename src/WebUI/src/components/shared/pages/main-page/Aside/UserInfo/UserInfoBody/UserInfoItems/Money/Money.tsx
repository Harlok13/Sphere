import style from "../UserInfoItems.module.css";
import React, {FC} from "react";

export const Money: FC<{
    money: number;
}> = ({money}) => {
    return (
        <li className={style.item}>
            <span className="material-icons-outlined">attach_money</span>
            <h4 className={style.moneyCount}>{money}</h4>
        </li>
    )
}