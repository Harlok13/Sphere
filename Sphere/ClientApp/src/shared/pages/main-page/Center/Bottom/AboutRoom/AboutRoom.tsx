import {PropsWithChildren} from "react";
// @ts-ignore
import style from "./AboutRoom.module.css";

export const AboutRoom = ({children}: PropsWithChildren) => {
    return (
        <div className={style.aboutRoom}>
            {children}
        </div>
    )
}