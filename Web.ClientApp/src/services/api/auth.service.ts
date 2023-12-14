import axios from "axios";
import UserService from "../user/user.service";
import {IAuthResponse} from "../../interfaces/auth/auth-response.types";


class AuthService {
    private readonly URL = "https://localhost:7170/auth";  // TODO: secrets, relocate to settings

    async register({email, userName, password, passwordConfirm}): Promise<void> {
        await axios.post<IAuthResponse>(`${this.URL}/register`, {
            email: email, userName: userName, password: password, passwordConfirm: passwordConfirm
        }, {withCredentials: true, headers: {"Content-Type": "application/json"}}).then(response => {
            this.setUserData(response.data);
            console.log("User data successfully received and saved.");
        }).catch(error => console.error("An error occurring during registration.\n", error.toString()));
    };

    // async register({email, userName, password, passwordConfirm}){
    //     try{
    //         console.log(email, userName, password, passwordConfirm);
    //         const response = await axios.post("https://localhost:7170/auth/register", {
    //             email: email, userName: userName, password: password, passwordConfirm: passwordConfirm
    //         }, {headers: {"Content-Type": "application/json"}})
    //
    //     }
    //     catch(error){
    //         console.log(error);
    //     }
    // }

    async login({email, password}): Promise<void> {
        await axios.post<IAuthResponse>(`${this.URL}/login`, {
            email, password
        }, {withCredentials: true}).then(response => {
            this.setUserData(response.data);
        }).catch(error => console.error("An error occurring during sign in.\n", error.toString()));
    };

    async checkIsAuth(): Promise<boolean> {
        await axios.get(`${this.URL}/check_auth`, {
            headers: {"Authorization": `Bearer ${UserService.getUser().token}`},
            withCredentials: true
        }).catch(error => {
            console.error("an error occurred during the authorization check process.\n", error.toString())
            return false;
        });

        return true;
    };

    private setUserData(data: IAuthResponse) {
        console.log(data, "user data");
        localStorage.setItem("user", JSON.stringify({
            token: data.token,
            refreshToken: data.refreshToken,
            isLoggedIn: true,
            userId: data.playerId,
            userName: data.playerName,
            userAvatar: "/img/avatars/me.jpg",
            userStatistic: JSON.stringify(data.userStatistic)
        }));

        localStorage.setItem("token", data.token);

        console.log("User data successfully received and saved.");
    };
}

export default new AuthService();