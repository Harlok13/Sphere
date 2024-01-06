export interface IPlayerHistoryResponse{
    id: string;
    score: string;
    playedAt: Date;
    cardsPlayed: string;
    result: number | string;  // TODO: fix type, must be only string ("win" | "lose" | "draw")
}

export type PlayerHistory = IPlayerHistoryResponse;