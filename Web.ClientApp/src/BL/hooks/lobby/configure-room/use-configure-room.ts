import {ChangeEvent} from "react";
import {
    setHighBid,
    setLowBid,
    setMaxBid,
    setMediumBid,
    setMinBid,
    setRoomName,
    setRoomSize,
    setStartBid
} from "../../../slices/lobby.slice";
import {useDispatch, useSelector} from "react-redux";
import {useNavigate} from "react-router-dom";
import {
    useCreateRoomHub,
    useJoinToRoomHub,
    useTestMethodHub
} from "../../hub-connection/server-methods/server-methods";
import {NavigateEnum} from "../../../../constants/navigate.enum";
import {signalRConnection} from "../../../../App";
import {useHubMethod} from "react-use-signalr";

export const useConfigureRoom = () => {
    const dispatch = useDispatch();
    const navigate = useNavigate();

    // @ts-ignore
    const lobby = useSelector(state => state.lobby);

    // @ts-ignore
    const userInfo = useSelector(state => state.userInfo);

    const createRoom = useCreateRoomHub();
    const joinToRoom = useJoinToRoomHub();


    // const a = useHubMethod(signalRConnection, "GetTest");
    const a = useTestMethodHub();
    const createRoomHandler = async (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();

        console.log("inv")
        // const data = JSON.stringify(lobby.roomSettings);
        // await createRoom.invoke(data);
        // await createRoom.invoke(lobby.roomSettings, {
        //     playerId: userInfo.userId,
        //     playerName: userInfo.userName,
        //     avatar: userInfo.avatar,
        //     isLeader:true,
        //     readiness: false
        // });
        await createRoom.invoke(lobby.roomSettings, userInfo.userId, userInfo.userName);
        // await
        // console.log(userInfo.userId, userInfo.money);
        // await a.invoke(userInfo.userId);

        navigate(NavigateEnum.Room);  // TODO: check response. if not ok -> prohibit navigate
    }

    const minBidHandler = (e: ChangeEvent<HTMLInputElement>) => {
        dispatch(setMinBid(e.target.value));
    }

    const maxBidHandler = (e: ChangeEvent<HTMLInputElement>) => {
        dispatch(setMaxBid(e.target.value));
    }

    const startBidHandler = (e: ChangeEvent<HTMLInputElement>) => {
        dispatch(setStartBid(e.target.value));
    }

    const roomSizeHandler = (e: ChangeEvent<HTMLInputElement>) => {
        dispatch(setRoomSize(e.target.value));
    }

    const roomNameHandler = (e: ChangeEvent<HTMLInputElement>) => {
        dispatch(setRoomName(e.target.value));
    }

    const lowBidHandler = (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        dispatch(setLowBid())
    }

    const mediumBidHandler = (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        dispatch(setMediumBid());
    }

    const highBidHandler = (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        dispatch(setHighBid());
    }

    const handlers = {
        minBidHandler,
        maxBidHandler,
        startBidHandler,
        roomSizeHandler,
        roomNameHandler,
        createRoomHandler,
        lowBidHandler,
        mediumBidHandler,
        highBidHandler
    }

    const roomSettings = lobby.roomSettings;

    return {handlers, roomSettings}
}
