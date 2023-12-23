import {Card} from "./added-card-response";

export interface IPlayerResponse{
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
}

export type Player = IPlayerResponse;