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
}
