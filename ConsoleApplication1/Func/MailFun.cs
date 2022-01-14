using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    /// <summary>
    /// 測試
    /// </summary>
    class MailFun
    {
        public bool SendMail()
        {
            try
            {
                System.Net.Mail.MailMessage mm = new System.Net.Mail.MailMessage();
                mm.From = new System.Net.Mail.MailAddress("jianjhehong@azion.com.tw");

                mm.Subject = "test_subject";
                mm.Body = "test_body";
                mm.IsBodyHtml = true;
                mm.Bcc.Add("jianjhehong@azion.com.tw");

                using (var file = new System.Net.Mail.Attachment("C:\\Users\\jianjhehong\\Desktop\\svn.txt"))
                {
                    mm.Attachments.Add(file);

                    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587))
                    {
                        client.Credentials = new System.Net.NetworkCredential("", "");
                        client.EnableSsl = true;
                        client.Send(mm);

                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
