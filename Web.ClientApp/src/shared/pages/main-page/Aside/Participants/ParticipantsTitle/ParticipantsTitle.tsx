// @ts-ignore
import style from "./ParticipantsTitle.module.css";

export const ParticipantsTitle = ({roomName}) => {
    return (
        <div className={style.title}>
            Room: {roomName}
        </div>
    )
}