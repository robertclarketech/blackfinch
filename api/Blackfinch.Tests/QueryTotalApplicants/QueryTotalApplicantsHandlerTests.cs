using AutoFixture.Xunit2;
using Blackfinch.Domain.Entities;
using Blackfinch.Domain.Enums;
using Blackfinch.Infrastructure.Commands.QueryTotalApplicants;
using FluentAssertions;

namespace Blackfinch.Tests.QueryTotalApplicants;

public class QueryTotalApplicantsHandlerTests : TestBase
{
	private readonly CancellationToken _cancellationToken = CancellationToken.None;
	private readonly QueryTotalApplicantsHandler _handler;

	private readonly QueryTotalApplicantsRequest _validRequest = new();

	public QueryTotalApplicantsHandlerTests()
	{
		_handler = new QueryTotalApplicantsHandler(DbContext);
	}

	[Theory]
	[AutoData]
	public async Task GetsFromDatabase(Dictionary<LoanStatus, int> expected)
	{
		foreach (var (key, value) in expected)
		{
			for (var i = 0; i < value; i++)
			{
				DbContext.LoanApplications.Add(new LoanApplication
				{
					LoanStatus = key
				});
			}
		}
		await DbContext.SaveChangesAsync(_cancellationToken);
		
		var result = await _handler.Handle(_validRequest, _cancellationToken);

		result.Should().BeEquivalentTo(expected);
	}
}