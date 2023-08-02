using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RemindMe.Contracts.Classes;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RemindMe.Contracts.Services
{
   

    public class RemindMeSendGridService : IRemindMeSendGridService
    {
        private readonly ILogger _logger;
        private readonly ISendGridClient _sendGridClient;
        private readonly IConfiguration _config;
        private readonly ReminderDetails _reminderDetails;
        private readonly string _emailFrom;
        private readonly string _emailTo;


        public RemindMeSendGridService(
            ILogger<RemindMeSendGridService> logger,
            ISendGridClient sendGridClient,
            IConfiguration config,
            IOptions<ReminderDetails> reminderDetails)
        {
            _logger = logger;
            _sendGridClient = sendGridClient;
            _config = config;
            _reminderDetails = reminderDetails.Value;
        }      

        public async Task SendReminderEmail(string emailTo)
        {
            try
            {
                              
                var from = new EmailAddress(_reminderDetails.EmailFrom);
                var toList = new List<EmailAddress>();
                var emailToListSplit = emailTo.Split(',');

                foreach (var email in emailToListSplit)
                {
                    if(!string.IsNullOrEmpty(email))
                    {
                        toList.Add(new EmailAddress(email));
                    }
                }

                var msg = new SendGridMessage
                {
                    From = from,
                    Subject = $"Reminder For Follow up ℹ"
                };

                string reminderEmail = "Hi there,<br/><br/>Please check your inbox for any mails you need to follow up and/or confirm emails that have been sent.<br/><br/>Regards,<br/>The Email Company";

                msg.AddContent(MimeType.Html, reminderEmail);
                msg.AddTos(toList);

                var response = await _sendGridClient.SendEmailAsync(msg);
                _logger.LogInformation("Status code from SendGrid is: {statusCode}", response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception while sending Welcome email! Error: {exception}", ex.Message);
                throw;
            }

        }
    }
}
