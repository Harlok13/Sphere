import {Link} from "react-router-dom";
import style from "./NavItem.module.css";
import {CgProfile} from "@react-icons/all-files/cg/CgProfile";

export const NavItem = () => {
    return (
        <>
            <li className={style.item}>
                <Link className={style.link} to="/profile">
                    <CgProfile className={style.icon}/>
                    Profile
                </Link>
            </li>
            <li className={style.item}>
                <Link className={style.link} to="/settings">
                    <span className={`${style.icon} material-icons-outlined`}>
                        tune
                    </span>
                    Settings
                </Link>
            </li>
            <li className={style.item}>
                <Link className={style.link} to="/leaderboards">
                    <span className={`${style.icon} material-icons-outlined`}>
                        leaderboard
                    </span>
                    Leaderboards
                </Link>
            </li>
            <li className={style.item}>
                <Link className={style.link} to="/lobby">
                    <span className={`${style.icon} material-icons-outlined`}>
                        room_preferences
                    </span>
                    Lobby
                </Link>
            </li>
        </>
    )
}