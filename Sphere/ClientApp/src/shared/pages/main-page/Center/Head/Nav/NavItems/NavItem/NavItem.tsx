import {Link} from "react-router-dom";
// @ts-ignore
import style from "./NavItem.module.css";

export const NavItem = () => {
    return (
        <>
            <li className={style.item}>
                <a className={style.link} href="#">
                    <span className={`${style.icon} material-icons-outlined`}>
                        tune
                    </span>
                </a>
            </li>
            <li className={style.item}>
                <a className={style.link} href="#">
                    <span className={`${style.icon} material-icons-outlined`}>
                        leaderboard
                    </span>
                </a>
            </li>
            <li className={style.item}>
                <a className={style.link} href="#">
                    <span className={`${style.icon} material-icons-outlined`}>
                        groups
                    </span>
                </a>
            </li>
            <li className={style.item}>
                <Link className={style.link} to="/home">
                    <span className={`${style.icon} material-icons-outlined`}>
                        room_preferences
                    </span>
                </Link>
            </li>
        </>
    )
}