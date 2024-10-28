using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Infrastructure.Email
{
    public class EmailSettings
    {
        public string MailToAddress = @"mcnielv@gmail.com";
        public string MailFromAddress = @"info@aac.com";
        public bool UseSsl = true;
        public string UserName = @"info@MNV.org";
        public string Password = @"cl@ssroom";
        public string ServerName = @"smtp.gmail.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation { get; set; }
    }
}
