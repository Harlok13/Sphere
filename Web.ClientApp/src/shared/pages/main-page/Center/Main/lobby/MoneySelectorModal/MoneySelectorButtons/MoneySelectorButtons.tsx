import style from "./MoneySelectorButtons.module.css";
import {FC} from "react";
import {
    SelectStartMoneyHandlers
} from "../../../../../../../../BL/hooks/lobby/select-start-money/use-select-start-money";


// type ModalButtonHandlers = {
//     confirmHandler: (e: React.MouseEvent<HTMLButtonElement>) => void;
//     cancelHandler: (e: React.MouseEvent<HTMLButtonElement>) => void;
// }

interface IMoneySelectorButtonsProps {
    handlers: SelectStartMoneyHandlers
}

export const MoneySelectorButtons: FC<IMoneySelectorButtonsProps> = ({handlers}) => {
    return (
        <div>
            <button onClick={handlers.confirmHandler} className={`${style.button} ${style.confirm}`}>Confirm</button>
            <button onClick={handlers.cancelHandler} className={`${style.button} ${style.cancel}`}>Cancel</button>
        </div>
    )
}