import {PropsWithChildren} from "react";
import style from "./Aside.module.css";

const Aside = ({children}: PropsWithChildren<{}>) => {
    return (
        <section className={style.aside}>
            {children}
        </section>
    )
}

export {Aside};