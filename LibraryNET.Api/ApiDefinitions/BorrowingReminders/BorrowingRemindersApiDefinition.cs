using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Api.ApiDefinitions.BorrowingReminders.Handlers;
using LibraryNET.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryNET.Api.ApiDefinitions.BorrowingReminders;

// ReSharper disable once UnusedType.Global
public sealed class BorrowingRemindersApiDefinition : IApiDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var groupBuilder = app.MapGroup("borrowing-reminders");

        groupBuilder.MapGet("", GetPendingBorrowingReminders);

        groupBuilder.MapPatch("{userId:long}/{bookId:long}", MarkReminded);
    }

    public void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GetPendingBorrowingRemindersHandler>();
        serviceCollection.AddScoped<MarkRemindedHandler>();
    }

    private static async Task<IResult> GetPendingBorrowingReminders(GetPendingBorrowingRemindersHandler handler, CancellationToken cancellationToken)
        => await handler.Handle(cancellationToken);

    private static async Task<IResult> MarkReminded([FromRoute] long userId, [FromRoute] long bookId, MarkRemindedHandler handler, CancellationToken cancellationToken)
        => await handler.Handle(userId, bookId, cancellationToken);
}