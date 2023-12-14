import {API} from "./config/axios.config";
import {AxiosResponse} from "axios";
import {IRequestLobbyData, IResponseLobbyData} from "../../interfaces/lobby/lobby.types";

class LobbyService {
    async createLobby(lobbyData: IRequestLobbyData): Promise<IResponseLobbyData>{
        const {data} = await API.post<IRequestLobbyData, AxiosResponse<IResponseLobbyData>>(
            "/api/lobby",
            {lobbyData}
        );

        return data;
    };
}