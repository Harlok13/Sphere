export interface IAddedGameHistoryMessageResponse {
    type: string;
    currentTime: string;
    message: string;
    playerName?: string;
}

export type GameHistoryMessage = IAddedGameHistoryMessageResponse;