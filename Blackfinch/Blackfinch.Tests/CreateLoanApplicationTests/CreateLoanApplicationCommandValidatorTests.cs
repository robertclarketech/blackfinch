using Blackfinch.Infrastructure.Commands.CreateLoanApplication;
using FluentValidation.TestHelper;

namespace Blackfinch.Tests.CreateLoanApplicationTests;

public class CreateLoanApplicationCommandValidatorTests
{
	private readonly CreateLoanApplicationCommandValidator _validator = new();

	private readonly CreateLoanApplicationRequest _validRequest = new()
	{
		CreditScore = 999,
		AssetValuePence = 20000000,
		LoanAmountPence = 10000000
	};

	[Fact]
	public void FailWhenAllValuesAreDefault()
	{
		var request = new CreateLoanApplicationRequest();

		var result = _validator.TestValidate(request);

		result.ShouldHaveValidationErrorFor(x => x.CreditScore);
		result.ShouldHaveValidationErrorFor(x => x.LoanAmountPence);
		result.ShouldHaveValidationErrorFor(x => x.AssetValuePence);
	}
}