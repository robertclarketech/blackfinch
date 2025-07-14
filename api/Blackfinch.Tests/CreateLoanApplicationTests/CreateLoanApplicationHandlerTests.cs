using Blackfinch.Domain.Entities;
using Blackfinch.Domain.Enums;
using Blackfinch.Infrastructure.Commands.CreateLoanApplication;
using FluentAssertions;
using Moq;

namespace Blackfinch.Tests.CreateLoanApplicationTests;

public class CreateLoanApplicationHandlerTests : TestBase
{
	private readonly CancellationToken _cancellationToken = CancellationToken.None;
	private readonly CreateLoanApplicationHandler _handler;

	private readonly CreateLoanApplicationRequest _validRequest = new()
	{
		CreditScore = 999,
		AssetValuePence = 20000000,
		LoanAmountPence = 10000000
	};

	public CreateLoanApplicationHandlerTests()
	{
		_handler = new CreateLoanApplicationHandler(DbContext, new CreateLoanApplicationCommandValidator());
	}

	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	[InlineData(long.MinValue)]
	public async Task FailWhenAssetValueIsZeroOrBelow(long assetValue)
	{
		_validRequest.AssetValuePence = assetValue;

		await _handler.Handle(_validRequest, _cancellationToken);

		DbContext.LoanApplications.Count().Should().Be(1);
		DbContext.LoanApplications.Single().LoanStatus.Should().Be(LoanStatus.Declined);
	}

	[Theory]
	[InlineData(1000)]
	[InlineData(int.MaxValue)]
	public async Task FailWhenCreditScoreAbove999(int creditScore)
	{
		_validRequest.CreditScore = creditScore;

		await _handler.Handle(_validRequest, _cancellationToken);

		DbContext.LoanApplications.Count().Should().Be(1);
		DbContext.LoanApplications.Single().LoanStatus.Should().Be(LoanStatus.Declined);
	}

	[Theory]
	[InlineData(9999999)]
	[InlineData(5000000)]
	[InlineData(100000)]
	[InlineData(0)]
	[InlineData(long.MinValue)]
	public async Task DeclineWhenLoanLessThan10000000(long loanAmount)
	{
		_validRequest.LoanAmountPence = loanAmount;

		await _handler.Handle(_validRequest, _cancellationToken);

		DbContext.LoanApplications.Count().Should().Be(1);
		DbContext.LoanApplications.Single().LoanStatus.Should().Be(LoanStatus.Declined);
	}

	[Theory]
	[InlineData(150000001)]
	[InlineData(500000000)]
	[InlineData(999999999)]
	[InlineData(long.MaxValue)]
	public async Task DeclineWhenLoanMoreThan150000000(long loanAmount)
	{
		_validRequest.LoanAmountPence = loanAmount;

		await _handler.Handle(_validRequest, _cancellationToken);

		DbContext.LoanApplications.Count().Should().Be(1);
		DbContext.LoanApplications.Single().LoanStatus.Should().Be(LoanStatus.Declined);
	}

	[Theory]
	[InlineData(160000000)]
	[InlineData(100000000)]
	[InlineData(10000000)]
	[InlineData(1)]
	public async Task DeclineWhenLoanIsAboveThanOneMillion_AndLtvLessThan60Percent(long assetValue)
	{
		_validRequest.LoanAmountPence = 100000000;
		_validRequest.AssetValuePence = assetValue;

		await _handler.Handle(_validRequest, _cancellationToken);

		DbContext.LoanApplications.Count().Should().Be(1);
		DbContext.LoanApplications.Single().LoanStatus.Should().Be(LoanStatus.Declined);
	}

	[Theory]
	[InlineData(949)]
	[InlineData(100)]
	[InlineData(1)]
	[InlineData(0)]
	[InlineData(-1)]
	[InlineData(int.MinValue)]
	public async Task DeclineWhenLoanIsAboveThanOneMillion_AndCreditScoreLessThan950(int creditScore)
	{
		_validRequest.LoanAmountPence = 100000000;
		_validRequest.CreditScore = creditScore;

		await _handler.Handle(_validRequest, _cancellationToken);

		DbContext.LoanApplications.Count().Should().Be(1);
		DbContext.LoanApplications.Single().LoanStatus.Should().Be(LoanStatus.Declined);
	}

	[Theory]
	[InlineData(950, 170000000)]
	[InlineData(999, 170000000)]
	public async Task ApproveWhenLoanIsAboveThanOneMillion_AndCreditScore950OrHigher_AndLtvIs60PercentOrHigher(
		int creditScore,
		long assetValue)
	{
		_validRequest.LoanAmountPence = 100000000;
		_validRequest.AssetValuePence = assetValue;
		_validRequest.CreditScore = creditScore;

		await _handler.Handle(_validRequest, _cancellationToken);

		DbContext.LoanApplications.Count().Should().Be(1);
		DbContext.LoanApplications.Single().LoanStatus.Should().Be(LoanStatus.Approved);
	}

	[Fact]
	public async Task ReturnId()
	{
		var response = await _handler.Handle(_validRequest, _cancellationToken);

		response.Should().Be(DbContext.LoanApplications.Single().Id);
	}
}