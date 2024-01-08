import style from "./LoginVia.module.css";

const LoginVia = () => {
    return (
        <div className={style.loginVia}>
            <div className={style.title}>Login via:</div>
            <ul className={style.items}>
                <li className={style.item}>
                    <a className={style.link} href="#">
                        <span className={`${style.icon} material-icons-outlined`}>
                            email
                        </span>
                    </a>
                </li>
                <li className={style.item}>
                    <a className={style.link} href="#">
                        <span className={`${style.icon} material-icons-outlined`}>
                            email
                        </span>
                    </a>
                </li>
                <li className={style.item}>
                    <a className={style.link} href="#">
                        <span className={`${style.icon} material-icons-outlined`}>
                            email
                        </span>
                    </a>
                </li>
                <li className={style.item}>
                    <a className={style.link} href="#">
                        <span className={`${style.icon} material-icons-outlined`}>
                            email
                        </span>
                    </a>
                </li>
            </ul>
        </div>
    )
}

export default LoginVia;