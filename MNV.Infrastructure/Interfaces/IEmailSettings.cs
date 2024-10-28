using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Infrastructure.Interfaces
{
    public interface IEmailSettings
    {
        string MailToAddress { get; }
        string MailFromAddress { get; }
        bool UseSsl { get; }
        string UserName { get; }
        string Password { get; }
        string ServerName { get; }
        int ServerPort { get; }
        bool WriteAsFile { get; }
        string FileLocation { get; set; }
    }
}
