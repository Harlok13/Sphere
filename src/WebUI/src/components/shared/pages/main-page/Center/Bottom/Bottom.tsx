import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./Bottom.module.css";

const Bottom = ({children}: PropsWithChildren<{}>) => {
    return (
        <div className={style.bottom}>
            {children}
        </div>
    )
}

export {Bottom};