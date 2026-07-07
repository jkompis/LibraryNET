using System;
using System.Threading.Tasks;
using LibraryNET.Scheduler.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace LibraryNET.Scheduler.Jobs;

[DisallowConcurrentExecution]
internal sealed class AutomaticReminderJob(
    IMailingService mailingService,
    IReminderEmailProvider reminderEmailProvider,
    IPendingBorrowingRemindersService pendingBorrowingRemindersService,
    ILogger<AutomaticReminderJob> logger)
    : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var pendingBorrowingReminders = await pendingBorrowingRemindersService.GetPendingBorrowingReminders();

        foreach (var pendingBorrowingReminder in pendingBorrowingReminders ?? [])
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                logger.LogDebug("Cancellation requested, stopping.");
                break;
            }

            try
            {
                logger.LogTrace("Sending reminder email to user {UserId} for book {BookId}", pendingBorrowingReminder.UserId, pendingBorrowingReminder.BookId);
                await mailingService.SendEmail(pendingBorrowingReminder.Email!, reminderEmailProvider.GetSubject(pendingBorrowingReminder), reminderEmailProvider.GetBody(pendingBorrowingReminder));
                await pendingBorrowingRemindersService.MarkReminded(pendingBorrowingReminder);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending reminder email to user {UserId} for book {BookId}", pendingBorrowingReminder.UserId, pendingBorrowingReminder.BookId);
            }
        }
    }
}