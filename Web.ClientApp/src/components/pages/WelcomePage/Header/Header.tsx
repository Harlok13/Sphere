import AuthNav from "../AuthNav/AuthNav";
import style from "./Header.module.css";

const Header = () => {
    return (
        <div className={style.head}>
            <AuthNav/>
        </div>
    )
}

export default Header;