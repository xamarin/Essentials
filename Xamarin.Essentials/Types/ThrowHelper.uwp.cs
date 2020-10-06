using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Xamarin.Essentials
{
    public static partial class ThrowHelper
    {
        public static T ThrowHttpRequestException<T>(string message) => throw new HttpRequestException(message);
    }
}
