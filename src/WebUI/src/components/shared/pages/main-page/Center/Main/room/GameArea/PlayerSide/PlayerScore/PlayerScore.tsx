import style from "./PlayerScore.module.css";

export const PlayerScore = ({score}) => {
    return (
        <div className={style.score}>
            {score}
        </div>
    )
}