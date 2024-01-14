// using App.Domain.Primitives;
// using App.Domain.Shared;
// using App.Domain.Shared.ResultImplementations;
//
// namespace App.Domain.ValueObjects;
//
// internal sealed class FirstName : ValueObject
// {
//     private const int MaxLength = 50;
//     
//     private FirstName(string value)
//     {
//         Value = value;
//     }
//     
//     public string Value { get; }
//
//     public static Result<FirstName> Create(string firstName)
//     {
//         if (string.IsNullOrWhiteSpace(firstName))
//         {
//             return InvalidResult<FirstName>.Create(
//                 new Error("First name is empty."));
//         }
//
//         if (firstName.Length > MaxLength)
//         {
//             return InvalidResult<FirstName>.Create(
//                 new Error("First name is too long."));
//         }
//
//         return SuccessResult<FirstName>.Create(firstName);
//     }
//
//     public override IEnumerable<object> GetAtomicValues()
//     {
//         yield return Value;
//     }
// }