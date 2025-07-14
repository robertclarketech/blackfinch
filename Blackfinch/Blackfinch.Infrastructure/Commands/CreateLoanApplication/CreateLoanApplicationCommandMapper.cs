using Blackfinch.Domain.Entities;
using Blackfinch.Domain.Enums;
using Riok.Mapperly.Abstractions;

namespace Blackfinch.Infrastructure.Commands.CreateLoanApplication;

[Mapper]
public static partial class CreateLoanApplicationCommandMapper
{
	[MapperIgnoreTarget(nameof(LoanApplication.Id))]
	public static partial LoanApplication ToLoanApplication(CreateLoanApplicationRequest request, LoanStatus loanStatus,
		string rejectionReason);
}