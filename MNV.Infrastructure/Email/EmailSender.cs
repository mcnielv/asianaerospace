using MNV.Infrastructure.Interfaces;
using MNV.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Infrastructure.Email
{
    public class EmailSender : IEmailSender
    {
        private EmailSettings _emailSettings;

        public EmailSender(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }
        
        public bool SendEmail(string to,string subject, string body)
        {
            bool messageSent;

            using (var smtpClient = new SmtpClient())
            {
                bool testMode;
                string applicationMode = "local";
                var bccAddress = string.Empty;
                try
                {
                    testMode = AppSettings.Setting<bool>("testMode");
                    applicationMode = AppSettings.Setting<string>("applicationMode");
                    bccAddress = AppSettings.Setting<string>("email.bcc");
                }
                catch (Exception)
                {
                    testMode = true;
                    applicationMode = "local";
                    bccAddress = "jayson.deasis@gaitsol.com";
                }               
                
                smtpClient.EnableSsl = _emailSettings.UseSsl;
                smtpClient.Host = _emailSettings.ServerName;
                smtpClient.Port = _emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials
                    = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password);
                if (_emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = _emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_emailSettings.MailFromAddress, "Adopt-A-Classroom");
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                if (!string.IsNullOrEmpty(to))
                {
                    if (testMode)
                    {
                        if (applicationMode.ToLower() == "local")
                        {
                            mailMessage.To.Add("jayson.deasis@gaitsol.com");
                        }                        
                    }
                    else
                    {
                        var recipients = to.Split(';');
                        foreach (var recipient in recipients)
                        {
                            if (!string.IsNullOrEmpty(recipient))
                            {
                                mailMessage.To.Add(recipient);
                            }
                        }
                    }

                    
                    if (!string.IsNullOrEmpty(bccAddress))
                    {
                        foreach (var address in bccAddress.Split(';'))
                        {
                            if (!string.IsNullOrEmpty(address))
                            {
                                mailMessage.Bcc.Add(new MailAddress(address));
                            }
                        }
                    }                    
                }                
                

                mailMessage.IsBodyHtml = true;

                if (_emailSettings.WriteAsFile)
                    mailMessage.BodyEncoding = Encoding.ASCII;

                smtpClient.Send(mailMessage);

                messageSent = true;
            }

            return messageSent;
        }

        public bool SendEmail(string to, string subject, string body, IEmailSettings settings)
        {
            return this.SendEmail(to, null, subject, body, settings);
        }

        public bool SendEmail(string to, string bcc, string subject, string body, IEmailSettings settings) {
            bool messageSent;

            using (var smtpClient = new SmtpClient())
            {
                bool testMode;
                string applicationMode = "local";
                var bccAddress = string.Empty;
                try
                {
                    testMode = AppSettings.Setting<bool>("testMode");
                    applicationMode = AppSettings.Setting<string>("applicationMode");
                    if (bcc == null)
                        bccAddress = AppSettings.Setting<string>("email.bcc");
                    else
                        bccAddress = bcc + ";" + AppSettings.Setting<string>("email.bcc"); // Added 4.12.2016 JJAC
                }
                catch (Exception)
                {
                    testMode = true;
                    applicationMode = "local";
                    bccAddress = "jayson.deasis@gaitsol.com";
                }

                smtpClient.EnableSsl = settings.UseSsl;
                smtpClient.Host = settings.ServerName;
                smtpClient.Port = settings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials
                    = new NetworkCredential(settings.UserName, settings.Password);
                if (settings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = settings.FileLocation;
                    smtpClient.EnableSsl = false;
                }


                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(settings.MailFromAddress, "Adopt-A-Classroom");
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                if (!string.IsNullOrEmpty(to))
                {
                    if (testMode)
                    {
                        if (applicationMode.ToLower() == "local")
                            mailMessage.To.Add("jayson.deasis@gaitsol.com");
                    }
                    else
                    {
                        var recipients = to.Split(';');
                        foreach (var recipient in recipients)
                        {
                            if (!string.IsNullOrEmpty(recipient))
                            {
                                mailMessage.To.Add(recipient);
                            }
                        }
                    }
               
                }
                if (!string.IsNullOrEmpty(bccAddress))
                {
                    foreach (var address in bccAddress.Split(';'))
                    {
                        if (!string.IsNullOrEmpty(address))
                        {
                            mailMessage.Bcc.Add(new MailAddress(address));
                        }
                    }
                }

                mailMessage.IsBodyHtml = true;

                if (settings.WriteAsFile)
                    mailMessage.BodyEncoding = Encoding.ASCII;

                smtpClient.Send(mailMessage);

                messageSent = true;
            }

            return messageSent;
        }
    }
}
