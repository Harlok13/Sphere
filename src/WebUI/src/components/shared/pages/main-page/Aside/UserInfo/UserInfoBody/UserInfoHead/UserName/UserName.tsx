import {Link} from "react-router-dom";
import style from "./UserName.module.css";
import {FC} from "react";
import {NavigateEnum} from "shared/constants/navigate.enum";

export const UserName: FC<{
    name: string;
}> = ({name}) => {
    return (
        <Link to={NavigateEnum.Profile}>
            <div className={style.username}>{name}</div>
        </Link>
    )
}