import {IPlayerDto} from "./player-dto";
import {IPlayerInfoResponse} from "./player-info-response";
import {IPlayerHistoryResponse} from "./player-history-response";
import {IRoomInLobbyDto} from "./room-in-lobby-dto";

export interface IInitDataResponse{
    player?: IPlayerDto;
    playerInfo?: IPlayerInfoResponse;
    playerHistories?: Array<IPlayerHistoryResponse>;
    rooms?: Array<IRoomInLobbyDto>;
}