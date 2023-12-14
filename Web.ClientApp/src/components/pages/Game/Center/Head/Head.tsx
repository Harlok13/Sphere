// @ts-ignore
import style from "./style.module.css";
import {Link} from "react-router-dom";

const Head = () => {
    return (
        <div className={style.head}>
            <nav className={style.nav}>
                <ul className={style.items}>
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
                </ul>
            </nav>
            <div className={style.lightToggle}>
                <span className={`${style.icon} material-icons-outlined`}></span>
            </div>
        </div>
    )
}

export default Head;