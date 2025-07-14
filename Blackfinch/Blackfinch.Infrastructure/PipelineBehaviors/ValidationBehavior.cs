using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Blackfinch.Infrastructure.PipelineBehaviors;

public interface IValidationRequestHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	ValidationResult[] ValidationResults { get; set; }
}

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : notnull
{
	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		var context = new ValidationContext<TRequest>(request);

		var validationResults = await Task.WhenAll(
			validators.Select(validator => validator.ValidateAsync(context, cancellationToken))
		);

		var errors = validationResults
			.Where(validationResult => !validationResult.IsValid)
			.SelectMany(validationResult => validationResult.Errors)
			.ToList();

		if (errors.Count != 0)
		{
			throw new ValidationException(errors);
		}

		return await next(cancellationToken);
	}
}