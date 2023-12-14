import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./Main.module.css";

const Main = ({children}: PropsWithChildren) => {
    return (
        <div className={style.main}>
            {children}
        </div>
    )
}

export {Main};