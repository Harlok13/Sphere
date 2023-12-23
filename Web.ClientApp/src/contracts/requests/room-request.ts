export interface IRoomRequest {
    roomName: string;
    roomSize: number;
    startBid: number;
    minBid: number;
    maxBid: number;
    avatarUrl: string;
    status: string;
    playersInRoom: number;
}