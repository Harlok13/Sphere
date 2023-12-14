// @ts-ignore
import style from "./style.module.css";
import {Link} from "react-router-dom";
import Logout from "./Logout/Logout";
import Level from "./Level/Level";
import Money from "./Money/Money";

const UserInfo = ({props}) => {

    // const user = JSON.parse(localStorage.getItem("user"));
    // const userStatistic = JSON.parse(user.userStatistic);

    return (
        <div className={style.userInfo}>
            <Link to="/profile">
                <img className={style.avatar} src="/img/avatars/me.jpg" alt="avatar"/>
            </Link>
            <div className={style.body}>
                <div className={style.head}>
                    <Link to="/profile">
                        <div className={style.username}>{props.userName}</div>
                    </Link>
                    <Logout/>
                </div>
                <ul className={style.items}>
                    <Money props={props}/>
                    <Level props={props}/>
                </ul>
            </div>
        </div>
    )
}

export default UserInfo;