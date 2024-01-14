import style from "./NotificationPanel.module.css";

const NotificationPanel = () => {
    return (
        <div className={style.notificationPanel}>
                <span className={`${style.icon} material-icons-outlined ${style.selected}`}>
                    mode_comment
                </span>
            <span className={`${style.icon} material-icons-outlined`}>
                    email
                </span>
            <span className={`${style.icon} material-icons-outlined`}>
                    notifications
                </span>
            <span className={`${style.icon} material-icons-outlined`}>
                    groups
                </span>
        </div>
    )
}

export {NotificationPanel};

