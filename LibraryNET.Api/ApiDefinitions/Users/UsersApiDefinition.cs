using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using LibraryNET.Api.ApiDefinitions.Users.Handlers;
using LibraryNET.Api.ApiDefinitions.Users.Requests;
using LibraryNET.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryNET.Api.ApiDefinitions.Users;

// ReSharper disable once UnusedType.Global
public sealed class UsersApiDefinition : IApiDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var groupBuilder = app.MapGroup("users");

        groupBuilder.MapGet("", GetAllUsers)
            .WithName("Get all users")
            .WithDescription("Gets all users");

        groupBuilder.MapGet("{id:long}", GetUser)
            .WithName("Get user by id")
            .WithDescription("Gets user by id");

        groupBuilder.MapPost("", CreateUser)
            .WithName("Create new user")
            .WithDescription("Creates a new user")
            .Produces<long>();

        groupBuilder.MapDelete("{id:long}", DeleteUser)
            .WithName("Delete user by id")
            .WithDescription("Deletes user by id");
    }

    public void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GetAllUsersHandler>();
        serviceCollection.AddScoped<GetUserHandler>();
        serviceCollection.AddScoped<CreateUserHandler>();
        serviceCollection.AddScoped<DeleteUserHandler>();
        
        
        serviceCollection.AddScoped<IValidator<RequestCreateUser>, RequestCreateUser.Validator>();
    }

    private static async Task<IResult> GetAllUsers(GetAllUsersHandler handler, CancellationToken cancellationToken)
        => await handler.Handle(cancellationToken);

    private static async Task<IResult> GetUser([FromRoute] long id, GetUserHandler handler, CancellationToken cancellationToken)
        => await handler.Handle(id, cancellationToken);

    private static async Task<IResult> CreateUser([FromBody] RequestCreateUser request, CreateUserHandler handler, CancellationToken cancellationToken)
        => await handler.Handle(request, cancellationToken);

    private static async Task<IResult> DeleteUser([FromRoute] long id, DeleteUserHandler handler, CancellationToken cancellationToken)
        => await handler.Handle(id, cancellationToken);
}