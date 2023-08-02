namespace RemindMe.Contracts.Services
{
    public interface IRemindMeSendGridService
    {
        Task SendReminderEmail(string emailTo);
    }
}