import style from "./Main.module.css";
import RegisterVia from "./RegisterVia/RegisterVia";
import {Link, useNavigate} from "react-router-dom";
import React, { useState} from "react";
import AuthService from "../../../../services/api/auth.service";

const Main = () => {
    const [userName, setUserName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [passwordConfirm, setPasswordConfirm] = useState("");

    const navigate = useNavigate();

    const submit = async (e) => {
        e.preventDefault();
        console.log({
            email, password, passwordConfirm
        })

        // const result = await AuthService.register({email: email, userName: userName, password: password, passwordConfirm: passwordConfirm})
        // if (result){
        //     await setUser({id: result.userId, userName: result.userName, isLoggedIn: true, userStatistic: {...result.userStatistic}});
        //     console.log(userName);
        //     navigate("/game");
        // }
        try{
            const result = await AuthService.register({email: email, userName: userName, password: password, passwordConfirm: passwordConfirm})
            console.log(result, "in main");
            // localStorage.setItem("isLoggedIn", true);
            // localStorage.setItem("userId", result.userId);
            // localStorage.setItem("userName", result.userName);
            // localStorage.setItem("userStatistic", JSON.stringify(result.userStatistic));
            // await setUser({id: result.userId, userName: result.userName, userStatistic: {...result.userStatistic}});
            setEmail("");
            setPassword("");
            navigate("/lobby");  // TODO: enums
        }
        catch(error){
            console.log(error);
        }

    };

    // if (redirect2){
    //     console.log("redirect")
    //     // return RedirectFunction("/lobby");
    //     redirect(redirect2);
    // }



    return (
        <>
            <div className={style.main}>
                <div className={style.title}>Register</div>
                <form onSubmit={submit} action="#" className={style.form}>
                    <div className={style.item}>
                        <label htmlFor="email" className={style.label}>Email</label>
                        <input
                            onChange={e => setEmail(e.target.value)}
                            id="email"
                            type="email"
                            className={style.input}
                            autoComplete="on"
                        />
                        <span className={`${style.icon} material-icons-outlined`}>
                    email
                </span>
                    </div>

                    <div className={style.item}>
                        <label htmlFor="username" className={style.label}>Username</label>
                        <input
                            onChange={e => setUserName(e.target.value)}
                            id="username"
                            type="text"
                            className={style.input}
                            autoComplete="on"
                        />
                        <span className={`${style.icon} material-icons-outlined`}>
                    email
                </span>
                    </div>

                    <div className={style.item}>
                        <label htmlFor="password" className={style.label}>Password</label>
                        <input
                            onChange={e => setPassword(e.target.value)}
                            id="password"
                            type="password"
                            className={style.input}
                            autoComplete="on"
                        />
                        <span className={`${style.icon} material-icons-outlined`}>
                    key
                </span>
                    </div>

                    <div className={style.item}>
                        <label htmlFor="passwordConfirm" className={style.label}>Confirm password</label>
                        <input
                            id="passwordConfirm"
                            type="password"
                            className={style.input}
                            onChange={e => setPasswordConfirm(e.target.value)}
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

                    <button id="submitRegister" className={style.button} type="submit" value="Register">Register</button>

                    <div className={style.register}>
                        Already have an account? <Link to="/login" className={style.link}>Log in</Link>
                    </div>
                </form>

                <RegisterVia />
            </div>

        </>
    )
}

export default Main;