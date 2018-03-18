using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Caboodle
{
    public static partial class DataTransfer
    {
        public static Task ShowShareUI(string text) =>
            ShowShareUI(new ShareTextRequest { Text = text });

        public static Task ShowShareUI(string text, string title) =>
            ShowShareUI(new ShareTextRequest { Text = text, Title = title });
    }

    public class ShareTextRequest
    {
        public string Title { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public string Uri { get; set; }
    }
}
