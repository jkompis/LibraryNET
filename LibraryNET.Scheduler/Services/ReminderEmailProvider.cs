namespace LibraryNET.Scheduler.Services;

internal interface IReminderEmailProvider
{
    string GetSubject(PendingBorrowingRemindersService.PendingBorrowingReminder pendingBorrowingReminder);
    string GetBody(PendingBorrowingRemindersService.PendingBorrowingReminder pendingBorrowingReminder);
}

internal sealed class ReminderEmailProvider : IReminderEmailProvider
{
    public string GetSubject(PendingBorrowingRemindersService.PendingBorrowingReminder pendingBorrowingReminder)
    {
        // TODO: implement
        return $"{pendingBorrowingReminder.UserName} {pendingBorrowingReminder.BookName}";
    }

    public string GetBody(PendingBorrowingRemindersService.PendingBorrowingReminder pendingBorrowingReminder)
    {
        // TODO: implement
        return $"{pendingBorrowingReminder.UserName} {pendingBorrowingReminder.BookName}";
    }
}