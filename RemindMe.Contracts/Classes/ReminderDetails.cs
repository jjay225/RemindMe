using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemindMe.Contracts.Classes
{
    public class ReminderDetails
    {
        public string SendGridEmailAddress { get; set; }
        public string SendGridEmailAddresses { get; set; }
        public string SendGridEmailName { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string SendGridApiKey { get; set; }
        public int WorkerDelayInMinutes { get; set; }
    }
}
