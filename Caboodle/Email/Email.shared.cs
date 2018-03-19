using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    /// <summary>
    /// Email class for
    /// </summary>
    public static partial class Email
    {
        public static Func<string, string> BodyFormatter
        {
            get;
            set;
        }

        static Email()
        {
            BodyFormatter = BodyFormatAsPlainText;

            return;
        }

        public static string BodyFormatAsPlainText(string body)
        {
            return body;
        }

        public static string GetMimeType(string s)
        {
            string mime_type;
            MimeTypes.TryGetValue(s, out mime_type);

            if (string.IsNullOrEmpty(mime_type))
            {
                return "application/octet-stream";
            }

            return mime_type;
        }

        public static Dictionary<string, string> MimeTypes
        {
            get;
            set;
        } = new Dictionary<string, string>()
        {
            { "text", "text/plain" },
            { "txt", "text/plain" },
            { "csv", "text/plain" },
            { "html", "text/html" },
            { "htm", "text/html" },
            { "jpg", "image/jpeg" },
            { "jpeg", "image/jpeg" },
            { "png", "image/png" },
            { "doc", "application/msword" },
            { "pdf", "application/pdf" },
            { "zip", "application/zip" },
            { "docx", "application/zip" },
            { "xlsx", "application/zip" },
            { "pptx", "application/zip" },
        };

        // TODO: check if there is posibility to get the info about status
        public static EmailSendEventHandler OnSendSuccess;

        // TODO: check if there is posibility to get the info about status
        public static EmailSendEventHandler OnSendError;
    }

    public partial class EmailMessage
    {
        public EmailMessage()
        {
        }

        public EmailMessage(string[] recipientsto, string subject, string body)
        {
        }

        public EmailMessage(EmailMessage message)
        {
        }

        public string[] RecipientsTo { get; set; }

        public string[] RecipientsCC { get; set; }

        public string[] RecipientsBCC { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }


        internal void Validate()
        {
            if (!(RecipientsTo?.Length > 0))
                throw new ArgumentException("Email recipients cannot be empty.", nameof(RecipientsTo));
        }
    }


    public delegate void EmailSendEventHandler(EmailSendEventArgs e);

    public class EmailSendEventArgs : EventArgs
    {
        public EmailSendEventArgs(double success, Exception exception)
        {
        }

        public bool Succes
        {
            get;
        }

        public Exception Exception
        {
            get;
        }
    }

    public class EmailException : Exception
    {
        public EmailException()
            : base()
        {
        }

        public EmailException(string message)
            : base(message)
        {
        }

        public EmailException(string message, Exception inner_exception)
            : base(message, inner_exception)
        {
        }
    }
}