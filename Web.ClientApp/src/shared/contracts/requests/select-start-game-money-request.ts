import {IRoomRequest} from "./room-request";

export interface ISelectStartGameMoneyRequest {
    roomRequest: IRoomRequest;
    playerId: string;
    roomId?: string;
}