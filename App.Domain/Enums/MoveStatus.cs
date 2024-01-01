namespace App.Domain.Enums;

public enum MoveStatus : byte
{
    None = 0,
    Fold = 1,
    Call = 2,
    Check = 3,
    Hit = 5,
    Stay = 7
}

// public static class MoveStatus
// {
//     public const int None = 1;
//     public const int Hit = 2;
// }