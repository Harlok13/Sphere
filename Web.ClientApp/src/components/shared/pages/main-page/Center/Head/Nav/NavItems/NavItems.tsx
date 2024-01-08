import {PropsWithChildren} from "react";
import style from "./NavItems.module.css";

export const NavItems = ({children}: PropsWithChildren) => {
    return (
        <ul className={style.items}>
            {children}
        </ul>
    )
}