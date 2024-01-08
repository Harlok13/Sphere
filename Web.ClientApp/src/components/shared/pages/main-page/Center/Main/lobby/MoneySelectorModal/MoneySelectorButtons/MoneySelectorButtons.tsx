import style from "./MoneySelectorButtons.module.css";
import {FC} from "react";
import {SelectStartMoneyHandlers} from "hooks/lobby/select-start-money/use-select-start-money";


export const MoneySelectorButtons: FC<{
    handlers: SelectStartMoneyHandlers;
}> = ({handlers}) => {
    return (
        <div>
            <button onClick={handlers.confirmHandler} className={`${style.button} ${style.confirm}`}>Confirm</button>
            <button onClick={handlers.cancelHandler} className={`${style.button} ${style.cancel}`}>Cancel</button>
        </div>
    )
}