import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./style.module.css";

export const Participants = ({children}: PropsWithChildren) => {
    return (
        <div className={style.container}>
            {children}
        </div>
    )
}