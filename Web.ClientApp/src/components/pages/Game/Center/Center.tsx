// @ts-ignore
import style from "./style.module.css"
import Bottom from "./Bottom/Bottom";
import Main from "./Main/Main";
import Head from "./Head/Head";

const Center = ({props}) => {
    return (
        <section className={style.center}>
            <Head/>
            <Main props={props}/>
            <Bottom props={props}/>
        </section>
    )
}

export default Center;