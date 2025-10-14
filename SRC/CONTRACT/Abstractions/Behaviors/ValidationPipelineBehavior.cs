using FluentValidation;
using COMMON.CONTRACT.Abstractions.Shared;
using MediatR;

namespace COMMON.CONTRACT.Abstractions.Behaviors;


public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // Nếu không có validator nào thì tiếp tục xử lý request
            if (!_validators.Any())
            {
                return await next();
            }

            // Thực hiện validation cho request
            Error[] errors = _validators
                .Select(v => v.Validate(request))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .Select(f => new Error(f.PropertyName, f.ErrorMessage))
                .Distinct()
                .ToArray();

            // Nếu có lỗi → trả kết quả thất bại
            if (errors.Any())
            {
                return CreateValidationResult<TResponse>(errors);
            }

            // Nếu không có lỗi → gọi handler tiếp theo
            return await next();
        }

        private static TRes CreateValidationResult<TRes>(Error[] errors)
            where TRes : Result
        {
            if (typeof(TRes) == typeof(Result))
            {
                return ValidationResult.WithErrors(errors) as TRes;
            }

            object validationResult = typeof(ValidationResult<>)
                .GetGenericTypeDefinition()
                .MakeGenericType(typeof(TRes).GenericTypeArguments[0])
                .GetMethod(nameof(ValidationResult.WithErrors))!
                .Invoke(null, new object?[] { errors })!;

            return (TRes)validationResult;
        }
    }