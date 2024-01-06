import {Link} from "react-router-dom";
import style from "./UserName.module.css";
import {FC} from "react";

interface IUserNameProps {
    name: string;
}

export const UserName: FC<IUserNameProps> = ({name}) => {
    return (
        <Link to="/profile">
            <div className={style.username}>{name}</div>
        </Link>
    )
}