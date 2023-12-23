import {IPlayerResponse} from "./player-response";

export interface IRoomResponse{
    id: string;
    roomName: string;
    roomSize: number;
    startBid: number;
    minBid: number;
    maxBid: number;
    avatarUrl: string;
    status: string | number;  // TODO: fix type, must be only string
    playersInRoom: number;
    bank: number;
    players: Array<IPlayerResponse>;
}

