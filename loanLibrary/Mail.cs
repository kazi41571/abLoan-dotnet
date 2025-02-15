using System;
using System.Net;
using System.Net.Mail;

namespace abHelper
{
    public class Mail
    {
        #region Properties
        public string FromMailAddress
        {
            get;
            set;
        }
        /// Example : Test1<test1@test.com>,Test2<test2@test.com>,test3@test.com
        /// , - Seperated
        public string ToMailAddresses
        {
            get;
            set;
        }
        public string CcMailAddresses
        {
            get;
            set;
        }
        public string BccMailAddresses
        {
            get;
            set;
        }
        public string ReplyToMailAddresses
        {
            get;
            set;
        }
        public string Subject
        {
            get;
            set;
        }
        public string Body
        {
            get;
            set;
        }
        public bool IsBodyHTML
        {
            get;
            set;
        }
        /// Example : Test1<c:\\Test1.txt>|Test2<c:\\Test2.txt>|c:\\Test2.txt
        /// | - Seperated
        public string AttachmentFilesWithPath
        {
            get;
            set;
        }
        public bool IsUseCredentials
        {
            get;
            set;
        }
        public string Host
        {
            get;
            set;
        }
        public int Port
        {
            get;
            set;
        }
        public bool IsSSL
        {
            get;
            set;
        }
        public string UserName
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        #endregion

        public void Send()
        {
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(this.FromMailAddress);
            mm.To.Add(ToMailAddresses);
            if (!string.IsNullOrEmpty(CcMailAddresses))
            {
                mm.CC.Add(CcMailAddresses);
            }
            if (!string.IsNullOrEmpty(BccMailAddresses))
            {
                mm.Bcc.Add(BccMailAddresses);
            }
            if (!string.IsNullOrEmpty(ReplyToMailAddresses))
            {
                mm.ReplyToList.Add(ReplyToMailAddresses);
            }
            mm.Subject = Subject;
            mm.IsBodyHtml = IsBodyHTML;
            mm.Body = Body;

            if (AttachmentFilesWithPath != null)
            {
                string[] FileNames = AttachmentFilesWithPath.Split('|');
                string AttachementFile;
                string Name;
                Attachment attachment;
                foreach (string FileName in FileNames)
                {
                    if (FileName.IndexOf('<') != -1)
                    {
                        Name = FileName.Substring(0, FileName.IndexOf('<'));
                        AttachementFile = FileName.Substring(FileName.IndexOf('<') + 1, FileName.Length - Name.Length - 2);
                    }
                    else
                    {
                        Name = string.Empty;
                        AttachementFile = FileName;
                    }
                    attachment = new Attachment(AttachementFile);
                    if (!string.IsNullOrEmpty(Name))
                    {
                        attachment.Name = Name;
                    }
                    mm.Attachments.Add(attachment);
                }
            }

            SmtpClient sc = new SmtpClient();
            sc.Host = Host;
            sc.Port = Port;
            sc.EnableSsl = IsSSL;
            if (IsUseCredentials)
            {
                NetworkCredential cred = new NetworkCredential(UserName, Password);
                sc.Credentials = cred;
            }
            sc.Send(mm);
        }
    }
}
