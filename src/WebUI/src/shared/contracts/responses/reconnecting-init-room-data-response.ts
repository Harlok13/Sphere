import {IPlayerDto} from "../player-dto";
import {IInitRoomDataResponse} from "../init-room-data-dto";
import {GameHistoryMessage} from "shared/contracts/responses/added-game-history-message-response";

export interface IReconnectingInitRoomDataResponse {
    player: IPlayerDto;
    players: Array<IPlayerDto>;
    gameHistory: Array<GameHistoryMessage>;
    initRoomData: IInitRoomDataResponse;
}