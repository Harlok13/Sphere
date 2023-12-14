import AuthNav from "../AuthNav/AuthNav";
// @ts-ignore
import style from "./style.module.css";

const Header = () => {
    return (
        <div className={style.head}>
            <AuthNav/>
        </div>
    )
}

export default Header;