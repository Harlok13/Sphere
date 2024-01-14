import {PropsWithChildren} from "react";
import style from "./Head.module.css";

const Head = ({children}: PropsWithChildren) => {
    return (
        <div className={style.head}>
            {children}
        </div>
    )
}

export {Head};