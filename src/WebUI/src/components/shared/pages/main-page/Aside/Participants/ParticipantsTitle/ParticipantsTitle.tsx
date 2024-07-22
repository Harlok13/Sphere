import style from "./ParticipantsTitle.module.css";
import {FC} from "react";


interface IParticipantsTitleProps {
    roomName: string;
}

export const ParticipantsTitle: FC<IParticipantsTitleProps> = ({roomName}) => {
    return (
        <div className={style.title}>
            Room: {roomName}
        </div>
    )
}