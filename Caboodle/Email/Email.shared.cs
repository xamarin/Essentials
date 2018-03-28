using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    public static partial class Email
    {
        public static Task ComposeAsync()
            => ComposeAsync(null);

        public static Task ComposeAsync(string subject, string body, params string[] to)
            => ComposeAsync(new EmailMessage(subject, body, to));

        public static Task ComposeAsync(EmailMessage message)
        {
            if (!IsComposeSupported)
                throw new FeatureNotSupportedException();

            return PlatformComposeAsync(message);
        }
    }

    public class EmailMessage
    {
        public EmailMessage()
        {
        }

        public EmailMessage(string subject, string body, params string[] to)
        {
            To = to?.ToList() ?? new List<string>();
        }

        public string Subject { get; set; }

        public string Body { get; set; }

        public List<string> To { get; set; }

        public List<string> Cc { get; set; }

        public List<string> Bcc { get; set; }

        public EmailBodyFormat BodyFormat { get; set; }

        public List<EmailAttachment> Attachments { get; set; }
    }

    public class EmailAttachment
    {
        public EmailAttachment()
        {
        }

        public EmailAttachment(string filePath)
        {
            Name = Path.GetFileName(filePath);
            FilePath = filePath;
        }

        public EmailAttachment(string name, string filePath)
        {
            Name = name;
            FilePath = filePath;
        }

        public EmailAttachment(string name, string filePath, string mimeType)
        {
            Name = name;
            FilePath = filePath;
            MimeType = mimeType;
        }

        public string Name { get; set; }

        public string MimeType { get; set; }

        public string FilePath { get; set; }
    }

    public enum EmailBodyFormat
    {
        PlainText,
        Html
    }
}
