export interface IUser {
    token: string;
    refreshToken: string;
    isLoggedIn: boolean;
    userId: number;
    userName: string;
    userAvatar: string;
    userStatistic: string;
}
