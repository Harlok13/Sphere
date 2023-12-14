// @ts-ignore
import style from "./UserInfoItems.module.css";
import {PropsWithChildren} from "react";

export const UserInfoItems = ({children}: PropsWithChildren) => {
    return (
        <div className={style.items}>
            {children}
        </div>
    )
}