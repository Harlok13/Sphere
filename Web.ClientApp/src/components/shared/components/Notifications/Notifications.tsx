import {PropsWithChildren} from "react";
import style from "./Notifications.module.css";
import {NotificationsList} from "./NotificationsList/NotificationsList";

export const Notifications = ({children}: PropsWithChildren) => {
    return (
        <div className={style.notifications}>
            <NotificationsList/>
            {children}
        </div>
    )
}
