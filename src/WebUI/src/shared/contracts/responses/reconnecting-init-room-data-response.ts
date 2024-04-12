import {IPlayerDto} from "../data/player-dto";
import {IInitRoomDataResponse} from "../data/init-room-data-dto";
import {GameHistoryMessage} from "shared/contracts/responses/room-responses/added-game-history-message-response";

export interface IReconnectingInitRoomDataResponse {
    player: IPlayerDto;
    players: Array<IPlayerDto>;
    gameHistory: Array<GameHistoryMessage>;
    initRoomData: IInitRoomDataResponse;
}