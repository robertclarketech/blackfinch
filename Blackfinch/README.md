# Blackfinch Technical Test Backend

## Setup

Please ensure you have the following installed:

- .NET 8 or higher
- A valid OCI runtime (docker or podman aliased to docker)

## Running the project

This project uses .NET Aspire for quick dev-time orchestration. In order to run this project, either:

- Run the Blackfinch.AppHost project in your IDE
- Type `dotnet run --project Blackfinch.AppHost` from the base directory of this repository.

Aspire should set up the necessary components to run in a developer environment, which includes a postgres database.

A Docker file is also provided.

The necessary configuration required to run the application standalone is:

- ConnectionStrings::postgresdb

## Things not implemented

- Automatic message brokering via rabbitmq upon successful command completion
- Integration tests via aspire
- Authorization