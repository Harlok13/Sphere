import style from "./MoneySelectorInput.module.css";
import {ChangeEvent, FC} from "react";
import {SelectStartGameMoney} from "shared/contracts/select-start-game-money-response";

interface IMoneySelectorInputProps {
    selector: SelectStartGameMoney;
    selectStartMoneyHandler: (e: ChangeEvent<HTMLInputElement>) => void;
}

export const MoneySelectorInput: FC<IMoneySelectorInputProps> = ({selector, selectStartMoneyHandler}) => {
    return (
        <div>
            <p>Select start money</p>
            {selector.lowerBound}
            <input
                type="range"
                onChange={selectStartMoneyHandler}
                value={selector.recommendedValue.toString()}
                min={selector.lowerBound.toString()}
                max={selector.upperBound.toString()}
            />
            {selector.upperBound}
            <div>{selector.recommendedValue}</div>
        </div>
    )
}