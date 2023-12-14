import style from "./ControlButton.module.css";
import {MouseEventHandler} from "react";

type ControlButtonProps = {
    clickHandler: MouseEventHandler;
    displayName: string;
}

export const ControlButton = (props: ControlButtonProps) => <button onClick={props.clickHandler} className={style.button}>{props.displayName}</button>
