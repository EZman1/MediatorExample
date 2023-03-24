using FluentValidation.Results;

namespace Application.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(object request, List<ValidationFailure> errors)
    {
        Errors = errors;
        Request = request;
    }

    public ValidationException(object request, List<ValidationFailure> errors, string[] ruleSetsExecuted) : this(request, errors)
    {
        RuleSetsExecuted = ruleSetsExecuted;
    }

    public object Request { get; }
    public List<ValidationFailure> Errors { get; }
    public string[] RuleSetsExecuted { get; } = Array.Empty<string>();
}