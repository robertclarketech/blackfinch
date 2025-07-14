using Blackfinch.Domain.Enums;
using Blackfinch.Infrastructure.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blackfinch.Infrastructure.Commands.QueryTotalApplicants;

public record QueryTotalApplicantsRequest : IRequest<Dictionary<LoanStatus, int>>;

public class QueryTotalApplicantsHandler(BlackfinchDbContext dbContext)
	: IRequestHandler<QueryTotalApplicantsRequest, Dictionary<LoanStatus, int>>
{
	public async Task<Dictionary<LoanStatus, int>> Handle(QueryTotalApplicantsRequest request,
		CancellationToken cancellationToken)
	{
		return await dbContext
			.LoanApplications
			.GroupBy(loanApplication => loanApplication.LoanStatus)
			.ToDictionaryAsync(grouping => grouping.Key, grouping => grouping.Count(), cancellationToken);
	}
}