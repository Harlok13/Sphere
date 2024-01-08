import {IPlayerDto} from "../player-dto";
import {IInitRoomDataResponse} from "../init-room-data-dto";

export interface ICreatedPlayerResponse {
    player: IPlayerDto;
    initRoomData: IInitRoomDataResponse;
    players: Array<IPlayerDto>;
}