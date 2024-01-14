import {PropsWithChildren} from "react";
import style from "./UserInfoHead.module.css";

export const UserInfoHead = ({children}: PropsWithChildren) => {
    return (
        <div className={style.head}>
            {children}
        </div>
    )
}