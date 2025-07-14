using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("dbUser", true);
var password = builder.AddParameter("dbPassword", true);

var postgres = builder.AddPostgres("postgres", username, password)
	.WithPgWeb();

var postgresdb = postgres.AddDatabase("postgresdb");

var api = builder.AddProject<Blackfinch_Api>("api")
	.WithReference(postgresdb)
	.WaitFor(postgresdb);

builder.Build().Run();