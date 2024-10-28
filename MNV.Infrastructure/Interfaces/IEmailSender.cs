using MNV.Infrastructure.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Infrastructure.Interfaces
{
    public interface IEmailSender
    {
        bool SendEmail(string to, string subject, string body);
        bool SendEmail(string to, string subject, string body, IEmailSettings settings);
        bool SendEmail(string to, string bcc, string subject, string body, IEmailSettings settings);
    }
}
