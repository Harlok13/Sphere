import {PropsWithChildren} from "react";
import style from "./Center.module.css";

const Center = ({children}: PropsWithChildren<{}>) => {
    return (
        <section className={style.center}>
            {children}
        </section>
    )
}

export {Center};