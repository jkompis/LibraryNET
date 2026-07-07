using System;
using LibraryNET.Scheduler.Jobs;
using LibraryNET.Scheduler.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

var builder = Host.CreateApplicationBuilder(args);

// register HTTP client to access borrowing reminders
var pendingBorrowingRemindersConfig = builder.Configuration.GetRequiredSection("PendingBorrowingRemindersApi");
builder.Services.AddHttpClient(PendingBorrowingRemindersService.HttpClientName, client =>
    client.BaseAddress = new Uri(pendingBorrowingRemindersConfig.GetValue<string>("BaseAddress")!));

builder.Services.AddSingleton<IMailingService, MailingService>();
builder.Services.AddSingleton<IReminderEmailProvider, ReminderEmailProvider>();
builder.Services.AddSingleton<IPendingBorrowingRemindersService, PendingBorrowingRemindersService>();

// configure scheduler
// TODO: load from configuration in the future
builder.Services.AddQuartz(q =>
{
    q.SchedulerId = "main-scheduler-id";
    q.SchedulerName = "Main Scheduler";

    q.UseInMemoryStore();
    q.UseSimpleTypeLoader();

    q.ScheduleJob<AutomaticReminderJob>(trigger =>
        trigger.WithIdentity(nameof(AutomaticReminderJob))
            .WithDescription("Automatic reminder job")
            .StartNow()
            // 8AM in CET/CEST
            .WithCronSchedule("0 0 8 * * ?", x => x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Europe/Bratislava"))));
});

builder.Services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });

var host = builder.Build();

await host.RunAsync();