using Blackfinch.Infrastructure.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blackfinch.Infrastructure.Commands.QueryMeanLtv;

public record QueryMeanLtvRequest : IRequest<decimal>;

public class QueryMeanLtvHandler(BlackfinchDbContext dbContext)
	: IRequestHandler<QueryMeanLtvRequest, decimal>
{
	public async Task<decimal> Handle(QueryMeanLtvRequest request,
		CancellationToken cancellationToken)
	{
		return await dbContext
			.LoanApplications
			.AverageAsync(application => (decimal?)application.LoanToValuePercent, cancellationToken) ?? 0;
	}
}