// @ts-ignore
import style from "../UserInfoItems.module.css";

export const Money = () => {
    return (
        <li className={style.item}>
            <span className="material-icons-outlined">attach_money</span>
            <h4 className={style.moneyCount}>560</h4>
        </li>
    )
}