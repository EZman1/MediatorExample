using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ValidationException = Application.Exceptions.ValidationException;

namespace Application.Behaviour;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest
{
    private readonly IEnumerable<IValidator<TRequest>> validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        this.validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            foreach (var validator in validators)
            {
                var result = await validator.ValidateAsync(request);
                HandleValidationResult(request, result);
            }
        }

        return await next();
    }

    private static void HandleValidationResult(TRequest request, ValidationResult result)
    {
        if (!result.IsValid)
        {
            throw new ValidationException(request, result.Errors, result.RuleSetsExecuted);
        }
    }
}