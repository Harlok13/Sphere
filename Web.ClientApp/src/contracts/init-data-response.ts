import {IPlayerResponse} from "./player-response";
import {IPlayerInfoResponse} from "./player-info-response";
import {IPlayerHistoryResponse} from "./player-history-response";
import {IRoomResponse} from "./room-response";

export interface IInitDataResponse{
    playerResponse: IPlayerResponse;
    playerInfoResponse: IPlayerInfoResponse;
    playerHistoriesResponse: Array<IPlayerHistoryResponse>;
    roomsResponse: Array<IRoomResponse>;
}