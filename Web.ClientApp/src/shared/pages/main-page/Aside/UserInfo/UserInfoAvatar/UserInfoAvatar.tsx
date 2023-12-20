import {Link} from "react-router-dom";
import style from "./UserInfoAvatar.module.css";
import {FC} from "react";

interface IUserInfoAvatarProps {
    avatarUrl: string
}

export const UserInfoAvatar: FC<IUserInfoAvatarProps> = ({avatarUrl}) => {

    return (
        <Link to="/profile">
            <img className={style.avatar} src={avatarUrl} alt="avatar"/>
        </Link>
    )
}
