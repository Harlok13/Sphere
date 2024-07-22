import {IRoomRequest} from "./room-request";

export interface ICreateRoomRequest {
    roomRequest: IRoomRequest;
    playerId: string;
    selectedStartMoney: number;
    upperBound: number;
    lowerBound: number;
}