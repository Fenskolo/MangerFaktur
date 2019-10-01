using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace MailSender
{
    public class MS
    {
        public bool SendMail(string login, string haslo, string from, string to, List<string> atach, string message, string subject)
        {
            bool Sukces = false;

            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        mail.From = new MailAddress(from);
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(login, haslo);
                        smtp.Host = "smtp.gmail.com";
                        smtp.Timeout = 30000000;
                        mail.To.Add(new MailAddress(to));

                        mail.IsBodyHtml = true;
                        mail.Subject = subject;
                        mail.Body = message;

                        if (atach != null && atach.Count > 0)
                        {
                            foreach (string at in atach)
                            {
                                Attachment attachment = new Attachment(at);
                                mail.Attachments.Add(attachment);
                            }
                        }

                        smtp.Send(mail);
                        Sukces = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return Sukces;
        }
    }
}


