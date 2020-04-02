using System;
using System.Collections.Generic;
using System.Text;

namespace MessageManager
{
   public class EmailConfiguration
    {

        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; } = "Server message";

        public int Port { get; set; }
        public string SmtpServer { get; set; }
        public string Password { get; set; }
    }
}
