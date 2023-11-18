// @ts-ignore
import style from "./style.module.css";

export const PlayerScore = ({score}) => {
    return (
        <div className={style.score}>
            {score}
        </div>
    )
}