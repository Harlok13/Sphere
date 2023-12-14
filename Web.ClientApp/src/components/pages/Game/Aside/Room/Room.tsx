// @ts-ignore
import style from "./style.module.css";

const Room = () => {
    return (
        <div className={style.room}>
            <div className={style.title}>Room: Game21</div>
            <ul className={style.list}>
                <li className={style.item}><span className={`${style.iconLeader} material-icons-outlined`}>star</span> Harlok<span className={`${style.iconReadiness} material-icons-outlined`}>close</span></li>
                <li className={style.item}>Bot<span className={`${style.iconReadiness} material-icons-outlined`}>done</span></li>
            </ul>

            <div className={style.bottom}>
                <button className={`${style.leave} ${style.button}`}>Leave<span className={`${style.iconBottom} material-icons-outlined`}>logout</span></button>
                <button disabled={true} className={`${style.invite} ${style.button}`}>Invite<span className={`${style.iconBottom} material-icons-outlined`}>person_add</span></button>
            </div>
        </div>
    )
}

export default Room;