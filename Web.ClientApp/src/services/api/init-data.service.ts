import {API} from "./config/axios.config";
import UserService from "../user/user.service";
import axios from "axios";

class InitDataService {  // TODO: finish
    async getInitData(){
        // const {data} = await API.get(`api/init_data/${UserService.getUser().userId}`)
        const {data} = await axios.get(`https://localhost:7170/api/init_data/${UserService.getUser().userId}`)
        console.log(data);
        return data;
    }
}

export default new InitDataService();