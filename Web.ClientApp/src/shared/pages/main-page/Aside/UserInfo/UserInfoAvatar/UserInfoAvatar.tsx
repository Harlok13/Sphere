import {Link} from "react-router-dom";
import style from "./UserInfoAvatar.module.css";

export const UserInfoAvatar = ({props}) => {

    return (
        <Link to="/profile">
            <img className={style.avatar} src={props.avatar} alt="avatar"/>
        </Link>
    )
}
