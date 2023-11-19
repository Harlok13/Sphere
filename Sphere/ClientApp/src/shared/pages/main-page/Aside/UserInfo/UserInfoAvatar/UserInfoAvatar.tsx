import {Link} from "react-router-dom";
// @ts-ignore
import style from "./UserInfoAvatar.module.css";

export const UserInfoAvatar = () => {
    return (
        <Link to="/profile">
            <img className={style.avatar} src="/img/avatars/me.jpg" alt="avatar"/>
        </Link>
    )
}