// @ts-ignore
import style from "./style.module.css";
import LoginVia from "./LoginVia/LoginVia";
import {Link} from "react-router-dom";
import {useState} from "react";
import AuthService from "../../../../services/api/auth.service";
const Main = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    // const [redirect, setRedirect] = useState(false);

    const submit = async (e) => {
        e.preventDefault();

        await AuthService.login({email: email, password: password})

    };

    return (
        <div className={style.main}>
            <div className={style.title}>Login</div>
            <form onSubmit={submit} action="#" className={style.form}>
                <div className={style.item}>
                    <label htmlFor="email" className={style.label}>Email</label>
                    <input
                        id="email"
                        type="email"
                        className={style.input}
                        onChange={e => setEmail(e.target.value)}
                    />
                    <span className={`${style.icon} material-icons-outlined`}>
                        email
                    </span>
                </div>

                <div className={style.item}>
                    <label htmlFor="password" className={style.label}>Password</label>
                    <input
                        id="password"
                        type="password"
                        className={style.input}
                        onChange={e => setPassword(e.target.value)}
                    />
                    <span className={`${style.icon} material-icons-outlined`}>
                        key
                    </span>
                </div>

                <div>
                    <input id="rememberMe" className={style.checkbox} name="rememberMe" type="checkbox"/>
                    <label htmlFor="rememberMe" className={style.remember}>remember me</label>
                </div>

                <button
                    id="submitLogin"
                    className={style.button}
                    type="submit"
                >
                    Log in
                </button>

                <div className={style.register}>
                    Don't have a account? <Link to="/register" className={style.link}>Register</Link>
                </div>
            </form>

            <LoginVia />
        </div>
    )
}

export default Main;