import {IInitDataResponse} from "shared/contracts/init-data-response";
import axios from "axios";
import UserService from "services/user/user.service";

class InitDataService {  // TODO: finish
    // private readonly URL = "http://localhost:5083/api/init_data";  
    private readonly URL = "https://localhost:7170/api/init_data";  // TODO: relocate to settings

    async getInitData(): Promise<IInitDataResponse>{
        const {data} = await axios.get<IInitDataResponse>(`${this.URL}/${UserService.getUser().userId}`,{
            headers: {"Authorization": `Bearer ${UserService.getUser().token}`},
            withCredentials: true
        })

        return data;
    }
}

export default new InitDataService();