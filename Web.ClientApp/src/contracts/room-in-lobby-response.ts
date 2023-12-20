export interface IRoomInLobbyResponse{
    id: string;
    roomName: string;
    roomSize: number;
    startBid: number;
    minBid: number;
    maxBid: number;
    imgUrl: string
    status: string | number;  // TODO: fix type, must be only string ("waiting" | "full" | "playing")
    playersInRoom: number;
    bank: number;
}

export type Room = {
    id: string;
    roomName: string;
    roomSize: number;
    startBid: number;
    minBid: number;
    maxBid: number;
    imgUrl: string;
    status: string | number;  // TODO: fix type, must be only string
    playersInRoom: number;
    bank: number;
}