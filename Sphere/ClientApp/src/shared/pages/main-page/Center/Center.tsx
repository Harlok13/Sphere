import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./style.module.css";

const Center = ({children}: PropsWithChildren<{}>) => {
    return (
        <section className={style.center}>
            {children}
        </section>
    )
}

export {Center};