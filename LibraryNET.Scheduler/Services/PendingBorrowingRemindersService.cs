using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LibraryNET.Scheduler.Services;

internal interface IPendingBorrowingRemindersService
{
    Task<PendingBorrowingRemindersService.PendingBorrowingReminder[]?> GetPendingBorrowingReminders();
    Task MarkReminded(PendingBorrowingRemindersService.PendingBorrowingReminder pendingBorrowingReminder);
}

internal sealed class PendingBorrowingRemindersService(IHttpClientFactory httpClientFactory) : IPendingBorrowingRemindersService
{
    public const string HttpClientName = "PendingBorrowingRemindersService";

    public async Task<PendingBorrowingReminder[]?> GetPendingBorrowingReminders()
    {
        var httpClient = httpClientFactory.CreateClient(HttpClientName);
        return await httpClient.GetFromJsonAsync<PendingBorrowingReminder[]>("borrowing-reminders");
    }

    public async Task MarkReminded(PendingBorrowingReminder pendingBorrowingReminder)
    {
        var httpClient = httpClientFactory.CreateClient(HttpClientName);
        var httpResponse = await httpClient.PatchAsync($"borrowing-reminders/{pendingBorrowingReminder.UserId}/{pendingBorrowingReminder.BookId}", null);
        httpResponse.EnsureSuccessStatusCode();
    }

    internal sealed class PendingBorrowingReminder
    {
        public string? Email { get; set; }
        public long UserId { get; set; }
        public string? UserName { get; set; }
        public long BookId { get; set; }
        public string? BookName { get; set; }
    }
}