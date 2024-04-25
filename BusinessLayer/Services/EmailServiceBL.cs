using BusinessLayer.Interfaces;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class EmailServiceBL:IEmailServiceBL
    {
        private readonly IEmailServiceRL _emailServiceRL;

        public EmailServiceBL(IEmailServiceRL emailServiceRL)
        {
            _emailServiceRL = emailServiceRL;
        }

        public Task<int> SendEmailAsync(string toEmail, string subject, string body)
        {
            return _emailServiceRL.SendEmailAsync(toEmail, subject, body);
        }
    }
}
