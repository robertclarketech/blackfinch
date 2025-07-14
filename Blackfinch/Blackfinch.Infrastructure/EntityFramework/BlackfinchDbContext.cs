using Blackfinch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blackfinch.Infrastructure.EntityFramework;

public class BlackfinchDbContext : DbContext
{
	public BlackfinchDbContext()
	{
	}

	public BlackfinchDbContext(DbContextOptions<BlackfinchDbContext> options)
		: base(options)
	{
	}

	public virtual DbSet<LoanApplication> LoanApplications { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<LoanApplication>().HasKey(e => e.Id);
	}
}