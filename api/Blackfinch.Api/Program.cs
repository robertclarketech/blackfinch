using System.Net;
using System.Text.Json;
using Blackfinch.Infrastructure.Commands.CreateLoanApplication;
using Blackfinch.Infrastructure.Commands.QueryMeanLtv;
using Blackfinch.Infrastructure.Commands.QueryTotalApplicants;
using Blackfinch.Infrastructure.Commands.QueryTotalLoanValue;
using Blackfinch.Infrastructure.EntityFramework;
using Blackfinch.Infrastructure.PipelineBehaviors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blackfinch.Api;

public class Program
{
	private const string ConnectionName = "postgresdb";

	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddAuthorization();

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		builder.AddNpgsqlDataSource(ConnectionName);

		builder.Services.AddDbContext<BlackfinchDbContext>(options =>
		{
			options.UseNpgsql(builder.Configuration.GetConnectionString(ConnectionName));
		});

		builder.Services.AddValidatorsFromAssemblyContaining<CreateLoanApplicationCommandValidator>();

		builder.Services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssemblyContaining<CreateLoanApplicationHandler>();
			cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
		});

		builder.AddServiceDefaults();

		var app = builder.Build();

		// Do NOT do this in production - this is just for testing expediency
		using (var scope = app.Services.CreateScope())
		{
			scope.ServiceProvider.GetRequiredService<BlackfinchDbContext>().Database.EnsureCreated();
		}

		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();

		app.Use(async (context, next) =>
		{
			try
			{
				await next.Invoke();
			}
			catch (ValidationException exception)
			{
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
				// TODO: Don't spit out the full error object, just property name and error message
				await context.Response.WriteAsync(JsonSerializer.Serialize(exception.Errors));
			}
		});

		app.MapPost("/loan/apply",
			async (CreateLoanApplicationRequest req, ISender sender, CancellationToken cancellationToken) =>
				await sender.Send(req, cancellationToken));
		app.MapGet("/loan/total-applicants",
			async ([AsParameters] QueryTotalApplicantsRequest req, ISender sender, CancellationToken cancellationToken) =>
			await sender.Send(req, cancellationToken));
		app.MapGet("/loan/total-value",
			async ([AsParameters] QueryTotalLoanValueRequest req, ISender sender, CancellationToken cancellationToken) =>
			await sender.Send(req, cancellationToken));
		app.MapGet("/loan/mean-ltv",
			async ([AsParameters] QueryMeanLtvRequest req, ISender sender, CancellationToken cancellationToken) =>
			await sender.Send(req, cancellationToken));

		app.Run();
	}
}