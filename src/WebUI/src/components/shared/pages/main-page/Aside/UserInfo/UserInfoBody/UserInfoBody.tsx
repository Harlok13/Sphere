import style from "./UserInfoBody.module.css";
import {PropsWithChildren} from "react";

export const UserInfoBody = ({children}: PropsWithChildren<{}>) => {
    return (
        <div className={style.body}>
            {children}
        </div>
    )
}