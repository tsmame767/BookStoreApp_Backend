using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string FromEmail { get; set; }
    }

    public class PasswordResetModel
    {
        public string OTP {  get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
        //public string Token { get; set; }

    }
    public class PasswordReset
    {
        public string NewPassword { get; set; }
        public string Token { get; set; }

    }
}
