using System.Reflection;
using LibraryNET.Api.Extensions;
using LibraryNET.Data.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqliteDatabase(builder.Configuration);

// add minimal APIs and their dependencies
builder.Services.RegisterServices(Assembly.GetExecutingAssembly());

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// handle minimal APIs
app.SetupWebApplication();

app.UseHttpsRedirection();

await app.RunAsync();