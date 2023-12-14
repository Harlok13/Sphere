import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./ControlPanel.module.css";

const ControlPanel = ({children}: PropsWithChildren) => {
    return (
        <div className={style.controlPanel}>
            {children}
        </div>
    )
}

export {ControlPanel};