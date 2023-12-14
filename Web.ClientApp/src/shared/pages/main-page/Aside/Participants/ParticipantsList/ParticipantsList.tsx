import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./ParticipantsList.module.css";

export const ParticipantsList = ({children}: PropsWithChildren) => {
    return (
        <div className={style.list}>
            {children}
        </div>
    )
}