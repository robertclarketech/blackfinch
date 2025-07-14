using FluentValidation;

namespace Blackfinch.Infrastructure.Commands.CreateLoanApplication;

public class CreateLoanApplicationCommandValidator : AbstractValidator<CreateLoanApplicationRequest>
{
	// Lots of named constants so their purpose is more obvious for the reader
	// In reality, these would likely be database driven and would be retrieved before validation
	private const int TenMillion = 10000000;
	private const int OneHundredMillion = 100000000;
	private const int OneHundredFiftyMillion = 150000000;

	private const int MinCreditScore = 1;
	private const int MaxCreditScore = 999;

	public const string LoanApplicationRuleset = "LoanApplication";

	private readonly SortedDictionary<decimal, int> _ltvToCreditScore = new()
	{
		{ 60, 750 }, { 80, 800 }, { 90, 900 }
	};

	public CreateLoanApplicationCommandValidator()
	{
		RuleFor(command => command.LoanAmountPence)
			.NotEmpty();
		RuleFor(command => command.CreditScore)
			.NotEmpty();
		RuleFor(command => command.AssetValuePence)
			.NotEmpty();

		RuleSet(LoanApplicationRuleset, () =>
		{
			RuleFor(command => command.LoanAmountPence)
				.InclusiveBetween(TenMillion, OneHundredFiftyMillion);
			RuleFor(command => command.CreditScore)
				.InclusiveBetween(MinCreditScore, MaxCreditScore);
			RuleFor(command => command.AssetValuePence)
				.GreaterThan(0);

			When(command => command.LoanAmountPence >= OneHundredMillion, () =>
			{
				RuleFor(command => command.CreditScore).GreaterThanOrEqualTo(950);
				RuleFor(command => command.LoanToValuePercent).LessThanOrEqualTo(60);
			}).Otherwise(() =>
			{
				RuleFor(command => command.CreditScore)
					.Must((command, _) => HaveValidCreditScore(command.CreditScore, command.LoanToValuePercent))
					.WithMessage("Invalid Credit Score");
			});
		});
	}

	private bool HaveValidCreditScore(int creditScore, decimal ltv)
	{
		foreach (var key in _ltvToCreditScore.Keys)
		{
			if (ltv < key && creditScore >= _ltvToCreditScore[key])
			{
				return true;
			}
		}

		return false;
	}
}