import style from "../UserInfoItems.module.css";
import React, {FC, useEffect, useRef} from "react";

interface IMoneyProps {
    money: number;
}

export const Money: FC<IMoneyProps> = ({money}) => {
    return (
        <li className={style.item}>
            <span className="material-icons-outlined">attach_money</span>
            <h4 className={style.moneyCount}>{money}</h4>
        </li>
    )
}