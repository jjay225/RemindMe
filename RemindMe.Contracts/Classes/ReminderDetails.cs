using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemindMe.Contracts.Classes
{
    public class ReminderDetails
    {     
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string SendGridApiKey { get; set; }
        public int WorkerDelayInMinutes { get; set; }
    }
}
