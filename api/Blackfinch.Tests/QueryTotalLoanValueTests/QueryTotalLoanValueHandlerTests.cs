using AutoFixture.Xunit2;
using Blackfinch.Domain.Entities;
using Blackfinch.Domain.Enums;
using Blackfinch.Infrastructure.Commands.QueryTotalLoanValue;
using FluentAssertions;

namespace Blackfinch.Tests.QueryTotalLoanValueTests;

public class QueryTotalLoanValueHandlerTests : TestBase
{
	private readonly CancellationToken _cancellationToken = CancellationToken.None;
	private readonly QueryTotalLoanValueHandler _handler;

	private readonly QueryTotalLoanValueRequest _validRequest = new();

	public QueryTotalLoanValueHandlerTests()
	{
		_handler = new QueryTotalLoanValueHandler(DbContext);
	}

	[Theory]
	[AutoData]
	public async Task GetsFromDatabase(List<long> loanAmounts)
	{
		foreach (var loanAmount in loanAmounts)
		{
			DbContext.LoanApplications.Add(new LoanApplication
			{
				LoanAmountPence = loanAmount,
				LoanStatus = LoanStatus.Approved
			});
		}
		await DbContext.SaveChangesAsync(_cancellationToken);
		
		var result = await _handler.Handle(_validRequest, _cancellationToken);

		result.Should().Be(loanAmounts.Sum());
	}
	
	[Theory]
	[AutoData]
	public async Task ShouldOnlyIncludeApproved(List<long> approvedLoanAmounts, List<long> declinedLoanAmounts)
	{
		foreach (var loanAmount in approvedLoanAmounts)
		{
			DbContext.LoanApplications.Add(new LoanApplication
			{
				LoanAmountPence = loanAmount,
				LoanStatus = LoanStatus.Approved
			});
		}
		foreach (var loanAmount in declinedLoanAmounts)
		{
			DbContext.LoanApplications.Add(new LoanApplication
			{
				LoanAmountPence = loanAmount,
				LoanStatus = LoanStatus.Declined
			});
		}
		await DbContext.SaveChangesAsync(_cancellationToken);
		
		var result = await _handler.Handle(_validRequest, _cancellationToken);

		result.Should().Be(approvedLoanAmounts.Sum());
	}
}