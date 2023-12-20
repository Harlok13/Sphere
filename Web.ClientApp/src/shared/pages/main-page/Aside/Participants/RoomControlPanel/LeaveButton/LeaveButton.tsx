import style from "../RoomControlPanel.module.css";
import {useNavigate} from "react-router-dom";
import {NavigateEnum} from "../../../../../../../constants/navigate.enum";

export const LeaveButton = () => {
    const navigate = useNavigate();

    return (
        <button onClick={() => navigate(NavigateEnum.Lobby)} className={`${style.leave} ${style.button}`}>
            Leave
            <span className={`${style.iconBottom} material-icons-outlined`}>
                logout
            </span>
        </button>
    )
}