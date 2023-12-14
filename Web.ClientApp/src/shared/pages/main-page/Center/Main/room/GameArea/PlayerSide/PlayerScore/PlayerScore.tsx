// @ts-ignore
import style from "./PlayeScore.module.css";

export const PlayerScore = ({score}) => {
    return (
        <div className={style.score}>
            {score}
        </div>
    )
}