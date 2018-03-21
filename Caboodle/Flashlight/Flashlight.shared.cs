using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Caboodle
{
    public static partial class Flashlight
    {
    }

    internal class FlashlightException : Exception
    {
        public FlashlightException()
        {
        }

        public FlashlightException(string message)
            : base(message)
        {
        }

        public FlashlightException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
