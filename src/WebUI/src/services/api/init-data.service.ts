import {IInitDataResponse} from "shared/contracts/init-data-response";
import axios from "axios";
import UserService from "services/user/user.service";

class InitDataService {  // TODO: finish
    private readonly URL = "http://localhost:5083/api/init_data";  // TODO: relocate to settings

    async getInitData(): Promise<IInitDataResponse>{
        // const {data} = await API.get(`api/init_data/${UserService.getUser().userId}`)
        const {data} = await axios.get<IInitDataResponse>(`${this.URL}/${UserService.getUser().userId}`)

        return data;
    }
}

export default new InitDataService();