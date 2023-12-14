// @ts-ignore
import style from "../style.module.css";

const Money = ({props}) => {
    return (
        <li className={style.item}>
            <span className="material-icons-outlined">attach_money</span>
            <h4 className={style.moneyCount}>{props.money}</h4>
        </li>
    )
}

export default Money;