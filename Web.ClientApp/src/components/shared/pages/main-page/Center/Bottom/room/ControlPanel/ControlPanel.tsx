import {PropsWithChildren} from "react";
import style from "./ControlPanel.module.css";

export const ControlPanel = ({children}: PropsWithChildren) =>
        <div className={style.controlPanel}>
            {children}
        </div>

