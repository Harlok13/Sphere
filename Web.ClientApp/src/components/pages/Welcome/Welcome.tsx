import Main from "./Main/Main";
// @ts-ignore
import style from "./style.module.css"
import Header from "./Header/Header";

const Welcome = () => {
    return (
        <div className={style.container}>
            <Header/>
            <Main/>
        </div>
    )
}

export default Welcome;

