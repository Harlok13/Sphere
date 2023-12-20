import {API} from "./config/axios.config";
import UserService from "../user/user.service";
import axios from "axios";
import {IInitDataResponse} from "../../contracts/init-data-response";

class InitDataService {  // TODO: finish
    async getInitData(): Promise<IInitDataResponse>{
        // const {data} = await API.get(`api/init_data/${UserService.getUser().userId}`)
        const {data} = await axios.get<IInitDataResponse>(`https://localhost:7170/api/init_data/${UserService.getUser().userId}`)

        return data;
    }
}

export default new InitDataService();