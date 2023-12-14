import style from "../RoomControlPanel.module.css";

export const LeaveButton = () => {
    return (
        <button className={`${style.leave} ${style.button}`}>
            Leave
            <span className={`${style.iconBottom} material-icons-outlined`}>
                logout
            </span>
        </button>
    )
}