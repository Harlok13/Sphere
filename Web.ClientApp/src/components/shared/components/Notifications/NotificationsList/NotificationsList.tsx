import style from "./NotificationsList.module.css";
import {NotEnoughMoney} from "../NotEnoughMoney/NotEnoughMoney";
import {v4} from "uuid";
import {useNotificationsSelector} from "store/notifications/use-notifications-selector";

export const NotificationsList = () => {
    const notifications = useNotificationsSelector();  // TODO reloc

    return (
        <div className={style.list}>
            {notifications.length
                ? notifications.map(notificationData => (<NotEnoughMoney key={v4()} notificationData={notificationData}/>))
                : null
            }
        </div>
    )
}