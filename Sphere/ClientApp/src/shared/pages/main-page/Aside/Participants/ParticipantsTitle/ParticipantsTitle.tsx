// @ts-ignore
import style from "./style.module.css";

export const ParticipantsTitle = (roomName: string) => {
    return (
        <div className={style.title}>
            Room: {roomName}
        </div>
    )
}