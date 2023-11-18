import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./style.module.css";

const Aside = ({children}: PropsWithChildren<{}>) => {
    return (
        <section className={style.aside}>
            {children}
        </section>
    )
}

export {Aside};