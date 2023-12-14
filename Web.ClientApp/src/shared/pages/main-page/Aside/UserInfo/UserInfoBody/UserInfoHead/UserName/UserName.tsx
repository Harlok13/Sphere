import {Link} from "react-router-dom";
// @ts-ignore
import style from "./UserName.module.css";

export const UserName = ({props}) => {
    return (
        <Link to="/profile">
            <div className={style.username}>{props.userName}</div>
        </Link>
    )
}