import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./Nav.module.css";

export const Nav = ({children}: PropsWithChildren) => {
    return (
        <nav className={style.nav}>
            {children}
        </nav>
    )
}