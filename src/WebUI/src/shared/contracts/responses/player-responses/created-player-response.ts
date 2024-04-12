import {IPlayerDto} from "../../data/player-dto";
import {IInitRoomDataResponse} from "../../data/init-room-data-dto";

export interface ICreatedPlayerResponse {
    player: IPlayerDto;
    initRoomData: IInitRoomDataResponse;
    players: Array<IPlayerDto>;
}