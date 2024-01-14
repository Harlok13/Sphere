import {useNavigate} from "react-router-dom";
import {useState} from "react";
import style from "./Logout.module.css";

export const Logout = () => {
    const navigate = useNavigate();
    const [showButtons, setShowButtons] = useState(false);

    const handleLogout = () => {
        localStorage.removeItem("user");
        navigate("/");
    }

    const buttonToggleHandler = () => {
        setShowButtons(prev => !prev);
    }

    return(
        <>
            {showButtons
                ? (
                    <>
                        <button onClick={handleLogout} className={style.confirm}>
                        <span className={`${style.icon} material-icons-outlined`}>
                            done
                        </span>
                        </button>

                        <button onClick={buttonToggleHandler} className={style.cancel}>
                        <span className={`${style.icon} material-icons-outlined`}>
                            close
                        </span>
                        </button>
                    </>
                )
                : (<span onClick={buttonToggleHandler} className={`${style.logoutIcon} material-icons-outlined`}>logout</span>)
            }
        </>

    )
}