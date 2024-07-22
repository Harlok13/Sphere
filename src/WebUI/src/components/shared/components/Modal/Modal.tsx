import style from "./Modal.module.css";
import {Transition} from "react-transition-group";
import {FC, ReactNode} from "react";

export const Modal: FC<{
    isOpen: boolean;
    onClose: () => void;
    children: ReactNode;
    timeout: number;
}> = ({isOpen, onClose, children, timeout}) => {
    const onWrapperClick = (e) => {
        if (e.target.classList.contains(style.wrapper)) onClose();
    }

    return (
        <>
            <Transition in={isOpen} timeout={timeout} unmountOnExit={true}>
                {state => (<div className={`${style.modal} ${style[state]}`}>
                    <div className={style.wrapper} onClick={onWrapperClick}>
                        <div className={style.content}>
                            <button className={style.closeButton} onClick={onClose}>
                                close
                            </button>
                            {children}
                        </div>
                    </div>
                </div>)}
            </Transition>
        </>
    )
}
