using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IEmailServiceBL
    {
        Task<int> SendEmailAsync(string toEmail, string subject, string body);
    }
}
