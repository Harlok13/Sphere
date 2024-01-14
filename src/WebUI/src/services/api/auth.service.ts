import axios from "axios";
import {IAuthResponse} from "shared/interfaces/auth/auth-response.types";
import UserService from "services/user/user.service";


class AuthService {
    // private readonly URL = "/api/auth";
    private readonly URL = "https://localhost:7170/api/auth";  
    // private readonly URL = "http://localhost:5083/api/auth";  // TODO: relocate to settings

    async register({email, userName, password, passwordConfirm}): Promise<void> {
        await axios.post<IAuthResponse>(`${this.URL}/register`, {
            email: email, userName: userName, password: password, passwordConfirm: passwordConfirm
        }, {withCredentials: true, headers: {"Content-Type": "application/json"}}).then(response => {
            this.setUserData(response.data);
            console.log("User data successfully received and saved.");
        }).catch(error => console.error("An error occurring during registration.\n", error.toString()));
    };

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
            console.error("An error occurred during the authorization check process.\n", error.toString())
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