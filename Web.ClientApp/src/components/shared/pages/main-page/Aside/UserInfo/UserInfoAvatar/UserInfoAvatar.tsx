import {Link} from "react-router-dom";
import style from "./UserInfoAvatar.module.css";
import {FC} from "react";
import {NavigateEnum} from "shared/constants/navigate.enum";

export const UserInfoAvatar: FC<{
    avatarUrl: string;
}> = ({avatarUrl}) => {

    return (
        <Link to={NavigateEnum.Profile}>
            <img className={style.avatar} src={avatarUrl} alt="avatar"/>
        </Link>
    )
}
