import style from "./ControlButton.module.css";
import {MouseEventHandler} from "react";

type ControlButtonProps = {
    clickHandler: MouseEventHandler;
    displayName: string;
    disabled: boolean
}

export const ControlButton = (props: ControlButtonProps) =>
    <button
        disabled={props.disabled}
        onClick={props.clickHandler}
        className={style.button}
    >
        {props.displayName}
    </button>
