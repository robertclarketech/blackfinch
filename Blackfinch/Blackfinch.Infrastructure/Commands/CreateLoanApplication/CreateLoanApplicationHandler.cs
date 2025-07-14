using Blackfinch.Domain.Enums;
using Blackfinch.Infrastructure.EntityFramework;
using FluentValidation;
using MediatR;

namespace Blackfinch.Infrastructure.Commands.CreateLoanApplication;

public record CreateLoanApplicationRequest : IRequest<Guid>
{
	public long LoanAmountPence { get; set; }
	public long AssetValuePence { get; set; }
	public int CreditScore { get; set; }
	public decimal LoanToValuePercent => AssetValuePence != 0 ? (decimal)LoanAmountPence / AssetValuePence * 100 : 0;
}

public class CreateLoanApplicationHandler(
	BlackfinchDbContext dbContext,
	CreateLoanApplicationCommandValidator validator)
	: IRequestHandler<CreateLoanApplicationRequest, Guid>
{
	public async Task<Guid> Handle(CreateLoanApplicationRequest request, CancellationToken cancellationToken)
	{
		var validationResult = await validator.ValidateAsync(request,
			options => options.IncludeRuleSets(CreateLoanApplicationCommandValidator.LoanApplicationRuleset),
			cancellationToken);

		var loanStatus = validationResult.IsValid ? LoanStatus.Approved : LoanStatus.Declined;
		var rejectionReason = string.Join(",", validationResult.Errors);
		var loanApplication =
			CreateLoanApplicationCommandMapper.ToLoanApplication(request, loanStatus, rejectionReason);

		dbContext.LoanApplications.Add(loanApplication);
		await dbContext.SaveChangesAsync(cancellationToken);
		return loanApplication.Id;
	}
}