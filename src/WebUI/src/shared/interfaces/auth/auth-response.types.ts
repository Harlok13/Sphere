export interface IAuthResponse {
    playerId: number;
    playerName: string;
    email: string;
    token: string;
    refreshToken: string;
    userStatistic: string;  // IUserStatistic
}