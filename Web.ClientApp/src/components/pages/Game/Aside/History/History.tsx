// @ts-ignore
import style from "./style.module.css"
import Head from "./Head/Head";
import Items from "./Items/Items";
import ShowMore from "./ShowMore/ShowMore";

const History = ({props}) => {

    return (
        <div className={style.history}>
            <Head/>
            <Items props={props}/>
            {props.userHistory.length > 5
                ? (<ShowMore/>)
                : null
            }

        </div>
    )
}

export default History;