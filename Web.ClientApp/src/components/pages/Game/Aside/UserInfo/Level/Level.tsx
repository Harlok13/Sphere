// @ts-ignore
import style from "../style.module.css";

const Level = ({props}) => {
    return (
        <li className={style.item}>
            <span className={`${style.levelIcon} material-icons-outlined`}>keyboard_double_arrow_up</span>
            <h4 className={style.level}>{props.level}</h4>
            <meter className={style.scale} value={props.exp} min="0" max="100"></meter>
        </li>
    )
}

export default Level;
