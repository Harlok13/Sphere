export interface IAddedCardResponse {
    playerId: string;
    cardDto: ICardDto;
}

export interface ICardDto {
    id: string;
    playerId: string;
    x: number;
    y: number;
    width: number;
    height: number;
    value: number;
    SuitValue: string;
}

export type Card = ICardDto;