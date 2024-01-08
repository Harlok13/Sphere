import {PropsWithChildren} from "react";
import style from "./UserInfo.module.css";

export const UserInfo = ({children}: PropsWithChildren<{}>) => {
    return (
        <div className={style.userInfo}>
            {children}
        </div>
    )
}