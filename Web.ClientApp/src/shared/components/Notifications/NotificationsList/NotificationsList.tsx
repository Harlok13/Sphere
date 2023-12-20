import style from "./NotificationsList.module.css";
import {NotEnoughMoney} from "../NotEnoughMoney/NotEnoughMoney";
import {useNotificationsSelector} from "../../../../BL/slices/notifications/use-notifications-selector";
import {v4} from "uuid";

export const NotificationsList = () => {
    const notifications = useNotificationsSelector();

    return (
        <div className={style.list}>
            {notifications.length
                ? notifications.map(notificationData => (<NotEnoughMoney key={v4()} notificationData={notificationData}/>))
                : null
            }
        </div>
    )
}