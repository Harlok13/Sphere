import Main from "./Main/Main";
import style from "./Welcome.module.css"
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

