using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailSender
{
    public class MS
    {
        public void SendMail(string login, string haslo, string from, string to, string[] atach)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        mail.From = new System.Net.Mail.MailAddress(from);
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(login, haslo);
                        smtp.Host = "smtp.gmail.com";

                        mail.To.Add(new MailAddress(to));

                        mail.IsBodyHtml = true;
                        mail.Subject = "temat";
                        mail.Body = "tresc";
                        if (atach != null && atach.Length > 0)
                        {
                            foreach (var at in atach)
                            {
                                Attachment attachment = new Attachment(at);
                                mail.Attachments.Add(attachment);
                            }
                        }

                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

           
