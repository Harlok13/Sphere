import style from "./Main.module.css";
import LoginVia from "./LoginVia/LoginVia";
import {Link, useNavigate} from "react-router-dom";
import {useRef, useState} from "react";
import AuthService from "../../../../services/api/auth.service";

const Main = () => {
    const emailRef = useRef();
    // const errRef = useRef();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    // const [errMsg, setErrMsg] = useState("");
    const navigate = useNavigate();

    // const [login, {isLoading}] = useLoginMutation();
    // const dispatch = useDispatch();
    // const {user, setUser} = useContext(UserContext);

    // useEffect(() => {
    //     emailRef.current.focus();
    // }, []);
    //
    // useEffect(() => {
    //     setErrMsg("");
    // }, [email, password]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        try{
            // const userData = await login({email, password}).unwrap();
            // dispatch(setCredentials({...userData, email}));
            const result = await AuthService.login({email: email, password: password});
            // console.log(result.userStatistic.draws);
            // localStorage.setItem("isLoggedIn", true);
            // localStorage("userId", result.userId);
            // localStorage("userName", result.userName);
            // localStorage("userStatistic", JSON.stringify(result.userStatistic));
            // await setUser({userStatistic: {...result.userStatistic}});
            setEmail("");
            setPassword("");
            navigate("/lobby");  // TODO: enums
        }
        catch(err){
            // if (!err?.response){
            //     setErrMsg("No Server Response");
            // } else if (err.response?.status === 400){
            //     setErrMsg("Missing Username or Password");
            // } else if (err.response?.status === 401) {
            //     setErrMsg("Unauthorized");
            // } else {
            //     setErrMsg("Login Failed");
            // }
            // errRef.current.focus();
            console.log(err);
        }
    }

    const handleEmailInput = (e) => setEmail(e.target.value);
    const handlePasswordInput = (e) => setPassword(e.target.value);



    return (
        <div className={style.main}>
            <div className={style.title}>Login</div>
            <form onSubmit={handleSubmit} className={style.form}>
                <div className={style.item}>
                    <label htmlFor="email" className={style.label}>Email</label>
                    <input
                        id="email"
                        type="email"
                        ref={emailRef}
                        value={email}
                        onChange={handleEmailInput}
                        className={style.input}
                        autoComplete="on"
                        required
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
                        onChange={handlePasswordInput}
                        value={password}
                        autoComplete="on"
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
                    Sign In
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