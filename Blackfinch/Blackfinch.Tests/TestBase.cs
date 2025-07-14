using Blackfinch.Domain.Entities;
using Blackfinch.Infrastructure.EntityFramework;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Blackfinch.Tests;

public class TestBase : IDisposable, IAsyncDisposable
{
	protected readonly BlackfinchDbContext DbContext;

	protected TestBase()
	{
		// Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
		// at the end of the test (see Dispose below).
		var connection = new SqliteConnection("Filename=:memory:");
		connection.Open();

		// These options will be used by the context instances in this test suite, including the connection opened above.
		var contextOptions = new DbContextOptionsBuilder<BlackfinchDbContext>()
			.UseSqlite(connection)
			.Options;

		// Create the schema and seed some data
		var context = new BlackfinchDbContext(contextOptions);
		context.Database.EnsureCreated();

		DbContext = context;
	}

	public void Dispose()
	{
		DbContext.Dispose();
	}

	public async ValueTask DisposeAsync()
	{
		await DbContext.DisposeAsync();
	}
}