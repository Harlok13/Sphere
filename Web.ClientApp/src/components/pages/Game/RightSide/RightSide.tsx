// @ts-ignore
import style from "./style.module.css"
import RoomChat from "./RoomChat/RoomChat";

const RightSide = ({props}) => {
    return (
        <>
            {props.showChat
                ? (
                    <section className={style.rightSide}>
                        <RoomChat/>
                    </section>
                )
                : null
            }
        </>
    )
}

export default RightSide;