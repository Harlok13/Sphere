import style from "./ContentContainer.module.css";
import {PropsWithChildren} from "react";

const ContentContainer = ({children}: PropsWithChildren<{}>) => {
    return (
        <div className={style.content}>
            {children}
        </div>
    )
}

export {ContentContainer};