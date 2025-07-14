using Blackfinch.Domain.Enums;
using Blackfinch.Infrastructure.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blackfinch.Infrastructure.Commands.QueryTotalLoanValue;

public record QueryTotalLoanValueRequest : IRequest<long>;

public class QueryTotalLoanValueHandler(BlackfinchDbContext dbContext)
	: IRequestHandler<QueryTotalLoanValueRequest, long>
{
	public async Task<long> Handle(QueryTotalLoanValueRequest request,
		CancellationToken cancellationToken)
	{
		return await dbContext
			.LoanApplications
			.Where(e => e.LoanStatus == LoanStatus.Approved)
			.SumAsync(application => application.LoanAmountPence, cancellationToken);
	}
}