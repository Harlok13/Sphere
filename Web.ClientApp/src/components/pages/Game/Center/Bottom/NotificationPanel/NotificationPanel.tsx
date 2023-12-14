// @ts-ignore
import style from "../style.module.css";

const NotificationPanel = ({props}) => {
    const toggleChatHandler = () => {
        props.setShowChat(prev => !prev);
    }

    return (
        <div className={style.notificationPanel}>
                <span onClick={toggleChatHandler} className={`${style.iconChat} material-icons-outlined`}>
                    mode_comment
                </span>
            <span className={`${style.iconMessages} material-icons-outlined`}>
                    email
                </span>
            <span className={`${style.iconNotifications} material-icons-outlined`}>
                    notifications
                </span>
            <span className={`${style.iconFriends} material-icons-outlined`}>
                    groups
                </span>
        </div>
    )
}

export {NotificationPanel};