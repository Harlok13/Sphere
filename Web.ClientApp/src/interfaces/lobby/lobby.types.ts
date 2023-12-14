export interface IRequestLobbyData {
    size: number;
    bid: number;
    startBid: number;
    name: string;
    imgUrl: string;
}

export interface IResponseLobbyData extends IRequestLobbyData {
    id: number;
}