// @ts-ignore
import style from "./style.module.css";

const RegisterVia = () => {
    return (
        <div className={style.registerVia}>
            <div className={style.title}>Register via:</div>
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

export default RegisterVia;

