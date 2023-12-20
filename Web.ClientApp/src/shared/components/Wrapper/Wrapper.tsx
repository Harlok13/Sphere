import style from "./Wrapper.module.css";
import {PropsWithChildren} from "react";

const Wrapper = ({children}: PropsWithChildren) => {
    return (
        <div className={style.wrapper}>
            {children}
        </div>
    );
}

export default Wrapper;