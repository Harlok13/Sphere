import style from "./NotEnoughMoney.module.css";
import { GoAlertFill } from "react-icons/go";
import {IoMdCloseCircle} from "react-icons/io";
import {NotEnoughMoneyNotification} from "../../../../contracts/not-enough-money-notification-response";
import React, {FC} from "react";
import {useDispatch} from "react-redux";
import {removeNotification} from "../../../../BL/slices/notifications/notifications";


interface INotEnoughMoneyProps {
    notificationData: NotEnoughMoneyNotification
}

export const NotEnoughMoney: FC<INotEnoughMoneyProps> = ({notificationData}) => {
    const dispatch = useDispatch();

    const closeHandler = (e: React.MouseEvent<SVGElement, MouseEvent>, id: string) => {
        e.preventDefault();

        dispatch(removeNotification(id))
    }

    return (
        <div className={style.body}>
            <GoAlertFill className={style.alert}/>
            <p>{notificationData.notificationText}</p>
            <IoMdCloseCircle onClick={(e) => closeHandler(e, notificationData.notificationId)} className={style.close}/>
        </div>
    )
}