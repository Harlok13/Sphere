import UserService from "../user/user.service";
import userService from "../user/user.service";
import axios from "axios";
import {IGame21Result} from "../../interfaces/game21/game-21-result.types";

// export const Game21Service = {
//     startGame: async (token, userId) => {
//         try {
//             console.log(userId, 'request id');
//             const response = await axios.get(`https://localhost:7087/api/game21/start_game/${userId}`, {
//                 headers: {"Content-Type": "application/json", "Authorization": `Bearer ${token}`}
//             });
//
//             return response.data;
//             // }
//         } catch (error) {
//             console.error(error.toString());
//             console.log("fetch 401");
//             const user = UserService.getUser();
//             const refresh = await axios.post("https://localhost:7087/auth/refresh_token", {"accessToken": user.token, "refreshToken": user.refreshToken}, {
//                 headers: {"Content-Type": "application/json", "Authorization": `Bearer ${user.token}`},
//             });
//             if (refresh.status === 200){
//                 user.token = refresh.data.accessToken;
//                 user.refreshToken = refresh.data.refreshToken;
//                 localStorage.setItem("user", JSON.stringify(user));
//                 console.log("refresh");
//                 const response = await axios.get("https://localhost:7087/api/game21/start_game/37", {
//                     headers: {"Content-Type": "application/json", "Authorization": `Bearer ${user.token}`}
//                 });
//                 return response.data;
//             }
//         }
//     },
//
//     pass: async (token) => {
//         try {
//             const response = await axios.get("https://localhost:7087/api/game21/pass", {
//                 headers: {"Content-Type": "application/json", "Authorization": `Bearer ${token}`}
//             });
//             return response.data;
//         } catch (error) {
//             console.error(error.toString());
//             console.log("fetch 401");
//             const user = UserService.getUser();
//             const refresh = await axios.post("https://localhost:7087/auth/refresh_token", {"accessToken": user.token, "refreshToken": user.refreshToken}, {
//                 headers: {"Content-Type": "application/json", "Authorization": `Bearer ${user.token}`},
//             });
//             if (refresh.status === 200){
//                 user.token = refresh.data.accessToken;
//                 user.refreshToken = refresh.data.refreshToken;
//                 localStorage.setItem("user", JSON.stringify(user));
//                 console.log("refresh");
//                 const response = await axios.get("https://localhost:7087/api/game21/pass", {
//                     headers: {"Content-Type": "application/json", "Authorization": `Bearer ${user.token}`}
//                 });
//                 return response.data;
//             }
//         }
//     },
//
//     getCard: async (token) => {
//         try {
//             const response = await axios.get("https://localhost:7087/api/game21/get_card",  {
//                 headers: {"Content-Type": "application/json", "Authorization": `Bearer ${token}`}
//             });
//             return response.data;
//         } catch (error) {
//             console.error(error.toString());
//             console.log("fetch 401");
//             const user = UserService.getUser();
//             const refresh = await axios.post("https://localhost:7087/auth/refresh_token", {"accessToken": user.token, "refreshToken": user.refreshToken}, {
//                 headers: {"Content-Type": "application/json", "Authorization": `Bearer ${user.token}`},
//             });
//             if (refresh.status === 200){
//                 user.token = refresh.data.accessToken;
//                 user.refreshToken = refresh.data.refreshToken;
//                 localStorage.setItem("user", JSON.stringify(user));
//                 console.log("refresh");
//                 const response = await axios.get("https://localhost:7087/api/game21/get_card",  {
//                     headers: {"Content-Type": "application/json", "Authorization": `Bearer ${user.token}`}
//                 });
//                 return response.data;
//             }
//         }
//     },
//
//     getStatistic: async (token, userId) => {
//         try {
//             const response = await axios.get(`https://localhost:7087/api/game21/get_statistic/${userId}`, {
//                 headers: {"Content-Type": "application/json", "Authorization": `Bearer ${token}`}
//             });
//             return response.data;
//         } catch (error) {
//             console.error(error.toString());
//             console.log("fetch 401");
//             const user = UserService.getUser();
//             const refresh = await axios.post("https://localhost:7087/auth/refresh_token", {"accessToken": user.token, "refreshToken": user.refreshToken}, {
//                 headers: {"Content-Type": "application/json", "Authorization": `Bearer ${user.token}`},
//             });
//             if (refresh.status === 200){
//                 user.token = refresh.data.accessToken;
//                 user.refreshToken = refresh.data.refreshToken;
//                 localStorage.setItem("user", JSON.stringify(user));
//                 console.log("refresh");
//                 const response = await axios.get(`https://localhost:7087/api/game21/get_statistic/${userId}`, {
//                     headers: {"Content-Type": "application/json", "Authorization": `Bearer ${user.token}`}
//                 });
//                 return response.data;
//             }
//         }
//     },
//
//     getHistory: async(token, userId) => {
//         try{
//             const response = await axios.get(`https://localhost:7087/api/game21/get_history/${userId}`,  {
//                 headers: {"Content-Type": "application/json", "Authorization": `Bearer ${token}`}
//             });
//             return response.data;
//         }
//         catch(error){
//             console.error(error.toString());
//             console.log("fetch 401");
//             const user = UserService.getUser();
//             const refresh = await axios.post("https://localhost:7087/auth/refresh_token", {"accessToken": user.token, "refreshToken": user.refreshToken}, {
//                 headers: {"Content-Type": "application/json", "Authorization": `Bearer ${user.token}`},
//             });
//             if (refresh.status === 200){
//                 user.token = refresh.data.accessToken;
//                 user.refreshToken = refresh.data.refreshToken;
//                 localStorage.setItem("user", JSON.stringify(user));
//                 console.log("refresh");
//                 const response = await axios.get(`https://localhost:7087/api/game21/get_history/${userId}`,  {
//                     headers: {"Content-Type": "application/json", "Authorization": `Bearer ${user.token}`}
//                 });
//                 return response.data;
//             }
//         }
//     }
// }

class Game21Service {
    private readonly URL: string = "https://localhost:7170/api/game21";
    constructor() {
        axios.defaults.baseURL = "https://localhost:7170";
        axios.defaults.headers.common["Authorization"] = `Bearer ${UserService.getUser().token}`;
        axios.defaults.headers.post["Content-Type"] = "application/json";
    }

    async startGame() {
        const response = await axios.get<IGame21Result>(
            `api/game21/start_game/${UserService.getUser().userId}`, {
                headers: {"Authorization": `Bearer ${UserService.getUser().token}`}
            })
            .catch(error => console.error("An error occurred during of starting the game.\n", error.toString()));

        if (response) {
            const data: IGame21Result = response.data;
            return data;
        }
    };

    async pass() {
        const response = await axios.get<IGame21Result>(`api/game21/pass`, {
            headers: {"Authorization": `Bearer ${UserService.getUser().token}`}
        })
            .catch(error => console.error("An error occurred during of pass.\n", error.toString()));

        if (response) {
            return response.data;
        }
    };

    async getCard() {
        const response = await axios.get<IGame21Result>(`api/game21/get_card`, {
            headers: {"Authorization": `Bearer ${UserService.getUser().token}`}
        })
            .catch(error => console.error("An error occurred during of get card.\n", error.toString()));

        if (response) {
            return response.data;
        }
    };

    async getStatistic() {
        const response = await axios.get<IGame21Result>(
            `${this.URL}/get_statistic/${UserService.getUser().userId}`, {
            headers: {"Content-Type": "application/json", "Authorization": `Bearer ${userService.getUser().token}`}
        }).catch(error => console.error("An error occurred during of get statistic.\n", error.toString()));

        if (response) {
            return response.data;
        }
    };

    async getHistory() {
        const response = await axios.get(
            `${this.URL}/get_history/${UserService.getUser().userId}`, {
            headers: {"Content-Type": "application/json", "Authorization": `Bearer ${UserService.getUser().token}`}
        }).catch(error => console.error("An error occurred during of get history.\n", error.toString()));

        if (response){
            return response.data;
        }
    }
}

export default new Game21Service();