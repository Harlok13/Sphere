export interface ICreatedRoomResponse {
    roomInLobbyDto: IRoomInLobbyDto;
}

export interface IRoomInLobbyDto {
    id: string;
    roomName: string;
    roomSize: number;
    startBid: number;
    minBid: number;
    maxBid: number;
    avatarUrl: string
    status: string | number;  // TODO: fix type, must be only string ("waiting" | "full" | "playing")
    playersInRoom: number;
    bank: number;
    lowerStartMoneyBound: number;
    upperStartMoneyBound: number;
}

export type Room = IRoomInLobbyDto;