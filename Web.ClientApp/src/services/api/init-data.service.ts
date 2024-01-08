import {IInitDataResponse} from "shared/contracts/init-data-response";
import axios from "axios";
import UserService from "services/user/user.service";

class InitDataService {  // TODO: finish
    async getInitData(): Promise<IInitDataResponse>{
        // const {data} = await API.get(`api/init_data/${UserService.getUser().userId}`)
        const {data} = await axios.get<IInitDataResponse>(`https://localhost:7170/api/init_data/${UserService.getUser().userId}`)

        return data;
    }
}

export default new InitDataService();