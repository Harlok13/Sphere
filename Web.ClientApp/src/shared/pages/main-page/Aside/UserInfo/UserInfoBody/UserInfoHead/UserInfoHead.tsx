import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./UserInfoHead.module.css";

export const UserInfoHead = ({children}: PropsWithChildren) => {
    return (
        <div className={style.head}>
            {children}
        </div>
    )
}