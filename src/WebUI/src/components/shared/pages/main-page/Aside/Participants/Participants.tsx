import {PropsWithChildren} from "react";
import style from "./Participants.module.css";

export const Participants = ({children}: PropsWithChildren) => {
    return (
        <div className={style.container}>
            {children}
        </div>
    )
}