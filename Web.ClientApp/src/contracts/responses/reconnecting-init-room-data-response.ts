import {IPlayerDto} from "../player-dto";
import {IInitRoomDataResponse} from "../init-room-data-dto";

export interface IReconnectingInitRoomDataResponse {
    player: IPlayerDto;
    players: Array<IPlayerDto>;
    initRoomData: IInitRoomDataResponse;
}