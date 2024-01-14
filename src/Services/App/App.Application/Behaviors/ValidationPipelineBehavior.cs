// using App.Domain.Shared;
// using FluentValidation;
// using Mediator;
//
// namespace App.Application.Behaviors;
//
// public class ValidationPipelineBehavior<TRequest, TResponse>
//     : IPipelineBehavior<TRequest, TResponse>
//     where TRequest : IRequest<TResponse>
//     where TResponse : Result
// {
//     private readonly IEnumerable<IValidator<TRequest>> _validators;
//
//     public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
//     {
//         _validators = validators;
//     }
//
//     public async ValueTask<TResponse> Handle(
//         TRequest message,
//         CancellationToken cT,
//         MessageHandlerDelegate<TRequest, TResponse> next)
//     {
//         if (!_validators.Any())
//         {
//             return await next(message, cT);
//         }
//
//         var errors = _validators
//             .Select(validator => validator.Validate(message))
//             .SelectMany(validationResult => validationResult.Errors)
//             .Where(validationFailure => validationFailure is not null)
//             .Select(failure => new Error(
//                 failure.ErrorMessage))
//             .Distinct()
//             .ToArray();
//
//         if (errors.Any())
//         {
//             return CreateValidationResult<TResponse>(errors);
//         }
//
//         return await next(message, cT);
//     }
//
//     private static TResult CreateValidationResult<TResult>(Error[] errors)
//         where TResult : Result
//     {
//         if (typeof(TResult) == typeof(Result))
//         {
//             return (ValidationResult.WithErrors(errors) as TResult)!;
//         }
//
//         object validationResult = typeof(ValidationResult<>)
//             .GetGenericTypeDefinition()
//             .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
//             .GetMethod(nameof(ValidationResult.WithErrors))!
//             .Invoke(null, new object?[] { errors })!;
//
//         return (TResult)validationResult;
//     }
// }