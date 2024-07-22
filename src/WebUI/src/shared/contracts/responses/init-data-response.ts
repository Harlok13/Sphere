import {IPlayerDto} from "../data/player-dto";
import {IPlayerInfoDto} from "../data/player-info-dto";
import {IPlayerHistoryDto} from "../data/player-history-dto";
import {IRoomInLobbyDto} from "../data/room-in-lobby-dto";

export interface IInitDataResponse{
    player?: IPlayerDto;
    playerInfo?: IPlayerInfoDto;
    playerHistories?: Array<IPlayerHistoryDto>;
    rooms?: Array<IRoomInLobbyDto>;
}