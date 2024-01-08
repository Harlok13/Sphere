import {Card} from "./responses/added-card-response";

export interface IPlayerDto {
    id: string;
    roomId: string;
    isLeader: boolean;
    readiness: boolean;
    playerName: string;
    score: number;
    avatarUrl: string;
    cards: Array<Card>;
    move: boolean;
    money: number;
    inGame: boolean;
    online: boolean;
}

export type Player = IPlayerDto;