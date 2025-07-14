using AutoFixture.Xunit2;
using Blackfinch.Domain.Entities;
using Blackfinch.Domain.Enums;
using Blackfinch.Infrastructure.Commands.QueryMeanLtv;
using Blackfinch.Infrastructure.Commands.QueryTotalApplicants;
using FluentAssertions;

namespace Blackfinch.Tests.QueryMeanLtv;

public class QueryMeanLtvHandlerTests : TestBase
{
	private readonly CancellationToken _cancellationToken = CancellationToken.None;
	private readonly QueryMeanLtvHandler _handler;

	private readonly QueryMeanLtvRequest _validRequest = new();

	public QueryMeanLtvHandlerTests()
	{
		_handler = new QueryMeanLtvHandler(DbContext);
	}

	[Theory]
	[InlineData(100, 0, 50)]
	[InlineData(100, 50, 75)]
	[InlineData(200, 100, 150)]
	[InlineData(200, 0, 100)]
	public async Task GetsFromDatabase(decimal firstLtv, decimal secondLtv, decimal expected)
	{
		DbContext.LoanApplications.AddRange(new LoanApplication
		{
			LoanToValuePercent = firstLtv
		},new LoanApplication
		{
			LoanToValuePercent = secondLtv
		});
		await DbContext.SaveChangesAsync(_cancellationToken);
		
		var result = await _handler.Handle(_validRequest, _cancellationToken);

		result.Should().Be(expected);
	}
}