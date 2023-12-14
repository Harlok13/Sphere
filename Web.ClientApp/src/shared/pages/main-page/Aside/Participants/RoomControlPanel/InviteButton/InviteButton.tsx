// @ts-ignore
import style from "../RoomControlPanel.module.css";

export const InviteButton = () => {
    return (
        <button disabled={true} className={`${style.invite} ${style.button}`}>
            Invite
            <span className={`${style.iconBottom} material-icons-outlined`}>
                person_add
            </span>
        </button>
    )
}