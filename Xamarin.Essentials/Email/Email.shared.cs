using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin.Essentials
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

        static string GetMailToUri(EmailMessage message)
        {
            if (message != null && message.BodyFormat != EmailBodyFormat.PlainText)
                throw new FeatureNotSupportedException("Only EmailBodyFormat.PlainText is supported if no email account is set up.");

            var parts = new List<string>();
            if (!string.IsNullOrEmpty(message?.Body))
                parts.Add("body=" + Uri.EscapeUriString(message.Body));
            if (!string.IsNullOrEmpty(message?.Subject))
                parts.Add("subject=" + Uri.EscapeUriString(message.Subject));
            if (message?.To.Count > 0)
                parts.Add("to=" + string.Join(",", message.To));
            if (message?.Cc.Count > 0)
                parts.Add("cc=" + string.Join(",", message.Cc));
            if (message?.Bcc.Count > 0)
                parts.Add("bcc=" + string.Join(",", message.Bcc));

            var uri = "mailto:";
            if (parts.Count > 0)
                uri += "?" + string.Join("&", parts);
            return uri;
        }
    }

    public class EmailMessage
    {
        public EmailMessage()
        {
        }

        public EmailMessage(string subject, string body, params string[] to)
        {
            Subject = subject;
            Body = body;
            To = to?.ToList() ?? new List<string>();
        }

        public string Subject { get; set; }

        public string Body { get; set; }

        public EmailBodyFormat BodyFormat { get; set; }

        public List<string> To { get; set; } = new List<string>();

        public List<string> Cc { get; set; } = new List<string>();

        public List<string> Bcc { get; set; } = new List<string>();

        public List<EmailAttachment> Attachments { get; set; } = new List<EmailAttachment>();
    }

    public enum EmailBodyFormat
    {
        PlainText,
        Html
    }

    public partial class EmailAttachment
    {
        internal const string DefaultContentType = "application/octet-stream";

        string filename;
        string contentType;

        public EmailAttachment(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("The attachment file path cannot be an empty string.", nameof(filePath));
            if (string.IsNullOrWhiteSpace(Path.GetFileName(filePath)))
                throw new ArgumentException("The attachment file path must be a file path.", nameof(filePath));

            FilePath = filePath;
        }

        public EmailAttachment(string filePath, string fileName, string contentType)
            : this(filePath)
        {
            FileName = fileName;
            ContentType = contentType;
        }

        public string FilePath { get; }

        public string FileName
        {
            get => GetFileName();
            set => filename = value;
        }

        public string ContentType
        {
            get => GetContentType();
            set => contentType = value;
        }

        internal string GetFileName()
        {
            // try the provided file name
            if (!string.IsNullOrWhiteSpace(filename))
                return filename;

            // try get from the path
            if (!string.IsNullOrWhiteSpace(FilePath))
                return Path.GetFileName(FilePath);

            // this should never happen as the path is validated in the constructor
            throw new InvalidOperationException($"Unable to determine the attachment file name from '{FilePath}'.");
        }

        internal string GetContentType()
        {
            // try the provided type
            if (!string.IsNullOrWhiteSpace(contentType))
                return contentType;

            // try get from the file extension
            var ext = Path.GetExtension(GetFileName());
            if (!string.IsNullOrWhiteSpace(ext))
            {
                var content = PlatformGetContentType(ext);
                if (!string.IsNullOrWhiteSpace(content))
                    return content;
            }

            // we haven't been able to determine this
            // leave it up to the sender
            return null;
        }
    }
}
