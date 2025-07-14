using Blackfinch.Domain.Enums;

namespace Blackfinch.Domain.Entities;

public class LoanApplication
{
	public Guid Id { get; set; }

	// TODO: Consider primitive obsession + future currencies; should maybe make a money type
	// int.MaxValue in pennies caps at around 15 million GBP while long.MaxValue at 68 quadrillion GBP
	// While decimals are generally considered suitable for financial and monetary calculations, I still prefer to use whole values when possible
	public long LoanAmountPence { get; set; }
	public long AssetValuePence { get; set; }
	public int CreditScore { get; set; }
	public decimal LoanToValuePercent { get; set; }
	public LoanStatus LoanStatus { get; set; }
	public string DeclineReason { get; set; } = string.Empty;
}