import Main from "./Main/Main";
import style from "./WelcomePage.module.css"
import Header from "./Header/Header";

const WelcomePage = () => {
    return (
        <div className={style.container}>
            <Header/>
            <Main/>
        </div>
    )
}

export default WelcomePage;

