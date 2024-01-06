import {IPlayerInfo} from "shared/interfaces/user/player-info.types";
import {IUser} from "shared/interfaces/user/user.types";


class UserService {
    // private readonly _user: IUser;
    // private readonly _userStatistic: IUserStatistic
    //
    // constructor() {
    //     const user: string = localStorage.getItem("user")
    //         ? localStorage.getItem("user")!
    //         : (() => {
    //             throw new Error("User data is empty.")
    //         })();
    //
    //     this._user = JSON.parse(user);
    //
    //     this._userStatistic = JSON.parse(this._user.userStatistic)
    // }
    private static readonly defaultUser: string = JSON.stringify({
        token: "",
        refreshToken: "",
        isLoggedIn: false,
        userId: 0,
        userName: "",
        userStatistic: ""
    });

    static getUser(): IUser {
        const user: string = localStorage.getItem("user")
            ? localStorage.getItem("user")!
            : (() => {
                return this.defaultUser;
            })();

        return JSON.parse(user);
    }

    static getUserStatistic(): IPlayerInfo {
        let user: string = localStorage.getItem("user")
            ? localStorage.getItem("user")!
            : (() => {
                throw new Error("User data is empty.")
            })();

        return JSON.parse(JSON.parse(user).userStatistic);
    }
}

export default UserService;