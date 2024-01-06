import {PropsWithChildren} from "react";
import style from "./RightSide.module.css";

const RightSide = ({children}: PropsWithChildren) => {
    return (
        <section className={style.rightSide}>
            {children}
        </section>
    )
}

export {RightSide};