import style from "./MoneySelectorModal.module.css"
import {PropsWithChildren} from "react";

export const MoneySelectorModal = ({children}: PropsWithChildren) => {
    return (
        <div className={style.moneySelector}>{children}</div>
    )
}