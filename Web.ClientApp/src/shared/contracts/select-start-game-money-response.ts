export interface ISelectStartGameMoneyResponse{
    roomId: string;
    lowerBound: number;
    upperBound: number;
    availableUpperBound: number;
    recommendedValue: number;
}

export type SelectStartGameMoney = ISelectStartGameMoneyResponse;