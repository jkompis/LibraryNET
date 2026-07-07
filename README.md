# This is just a demo project.

## Getting started

To start using it navigate to LibraryNET.Migrations and execute `dotnet ef database update` to create SQLite database.

## Projects overview

### LibraryNET.Api

Project containing API definitions.

### LibraryNET.Data

Project containing DB context and entities

### LibraryNET.Migrations

Project containing DB migrations, it serves to separate migrations from actual running code.

### LibraryNET.Scheduler

Project containing Quartz scheduler.

### LibraryNET.Tests

Project containing unit tests using xUnit framework.

## Services overview

### LibraryNET.Api

This service is main service providing basic library APIs. It utilizes minimal APIs and entity framework.

To use / test it you can access http://localhost:5224/swagger

### LibraryNET.Scheduler

This service is scheduler service that is dependent on LibraryNET.Api. Its purpose is to handle scheduling of tasks.