// @ts-ignore
import style from "./Wrapper.module.css";

const Wrapper = ({children}) => {
    return (
        <div className={style.wrapper}>
            {children}
        </div>
    );
}

export default Wrapper;