import UserInfo from "./UserInfo/UserInfo";
import History from "./History/History";
import Room from "./Room/Room";
import style from "./style.module.css";

const Aside = ({props}) => {
    return (
        <aside className={style.aside}>
            <UserInfo props={props}/>
            <History props={props}/>
            <Room/>
        </aside>
    )
}

export default Aside;