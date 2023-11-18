import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./style.module.css";

const Head = ({children}: PropsWithChildren) => {
    return (
        <div className={style.head}>
            {children}
        </div>
    )
}

export {Head};