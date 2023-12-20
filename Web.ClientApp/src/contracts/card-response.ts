export interface ICardResponse {
    id: string;
    playerId: string;
    x: number;
    y: number;
    width: number;
    height: number;
    value: number;
    SuitValue: string;
}

export type Card = ICardResponse;