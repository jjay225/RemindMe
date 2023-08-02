using Microsoft.Extensions.Options;
using RemindMe.Contracts.Classes;
using RemindMe.Contracts.Services;

namespace RemindMe.Reminder
{
    public class EmailWorker : BackgroundService
    {
        private readonly ILogger<EmailWorker> _logger;
        private readonly IConfiguration _config;
        private readonly IRemindMeSendGridService _RemindMeSendGridService;
        private readonly ReminderDetails _emailDetails;

        public EmailWorker(
            ILogger<EmailWorker> logger,
            IConfiguration config,
            IRemindMeSendGridService RemindMeSendGridService,
            IOptions<ReminderDetails> emailDetails)
        {
            _logger = logger;
            _config = config;
            _RemindMeSendGridService = RemindMeSendGridService;
            _emailDetails = emailDetails.Value;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reminder Service startup...");
          

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "In ExecuteAsync at: {time}, about to fire off a task in {minutes} minute(s).",
                    DateTimeOffset.Now,
                    _emailDetails.WorkerDelayInMinutes);
                await Task.Delay(TimeSpan.FromMinutes(_emailDetails.WorkerDelayInMinutes), stoppingToken);
                await SendReminderEmail();
            }
        }

        private async Task SendReminderEmail()
        {
            try
            {
                if(IsInWorkingHours())
                {
                    _logger.LogInformation("Sending Reminder email...");
                    await _RemindMeSendGridService.SendReminderEmail(_emailDetails.EmailTo);
                }
                else
                {
                    _logger.LogInformation("Outside of working hours, returning...");
                }
              
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception while attempting to send a Reminder email, error details: {error}", ex.Message);               
            }
        }

        private bool IsInWorkingHours()
        {
            DateTime now = DateTime.UtcNow;
            TimeSpan start = new TimeSpan(8, 0, 0); // 8 o'clock
            TimeSpan end = new TimeSpan(17, 30, 0); // 17:30 o'clock
            TimeSpan nowTime = now.TimeOfDay;

            if (now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            if (nowTime > start && nowTime < end)
            {
                _logger.LogInformation("Sending Reminder email...");
                return true;
            }

            return false;
        }

    }
}