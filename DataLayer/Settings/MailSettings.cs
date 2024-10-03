using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//"MailSettings": {
//    "Email": "abdallah3162@gmail.com",
//    "DisplayName": "Your Name",
//    "Password": "A951M951H951",
//    "Host": "smtp.gmail.com",
//    "Port": 587
//  }

namespace DataLayer.Settings
{
    public class MailSettings
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }

//    "Email": {
//    "SmtpHost": "smtp-mail.outlook.com",
//    "SmtpPort": "587",
//    "EmailSenderEmail": "abdallah11110000@outlook.com",
//    "EmailPassword": "f93f2270-e216-41f6-96ea-1b03aae6fae4",
//    "SmtpEnableSSL": false,
//    "EmailSenderName": "HrSystem",
//    "ConfirmationEmailSubject": "Your Smart Home Registration Confirmation"
//}

public class OutlookSettings
    {
        public string SmtpHost { get; set; }
        public string SmtpPort { get; set; }
        public string EmailSenderEmail { get; set; }
        public string EmailPassword { get; set; }
        public bool SmtpEnableSSL { get; set; }
        public string EmailSenderName { get; set; }
        public string ConfirmationEmailSubject { get; set; }


    }



}
