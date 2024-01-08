export interface IPlayerInfoResponse {
    id: string;
    avatarUrl: string;
    playerName: string;
    matches: number;
    loses: number;
    wins: number;
    draws: number;
    allExp: number;
    currentExp: number;
    targetExp: number;
    money: number;
    likes: number;
    level: number;
    has21: number;
}

export type PlayerInfo = IPlayerInfoResponse;