using System;
using System.IO;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class ThrowHelper
    {
#if !(NETSTANDARD1_0 || NETSTANDARD2_0)
        public static void ThrowNotImplementedException() => throw new NotImplementedException();

        public static T ThrowNotImplementedException<T>() => throw new NotImplementedException();
#else
        public static void ThrowNotImplementedException() => throw new NotImplementedException($"This API is not supported on {DeviceInfo.Platform}");

        public static T ThrowNotImplementedException<T>() => throw new NotImplementedException($"This API is not supported on {DeviceInfo.Platform}");
#endif

        public static void ThrowNotImplementedException(string message) => throw new NotImplementedException(message);

        public static T ThrowNotImplementedException<T>(string message) => throw new NotImplementedException(message);

        public static void ThrowIndexOutOfRangeException(string message) => throw new IndexOutOfRangeException(message);

        public static T ThrowIndexOutOfRangeException<T>(string message) => throw new IndexOutOfRangeException(message);

        public static void ThrowInvalidOperationException(string message) => throw new InvalidOperationException(message);

        public static T ThrowInvalidOperationException<T>(string message) => throw new InvalidOperationException(message);

        public static void ThrowArgumentNullException(string paramName, string message) => throw new ArgumentNullException(paramName, message);

        public static void ThrowArgumentException(string message, string paramName) => throw new ArgumentException(message, paramName);

        public static T ThrowArgumentException<T>(string message) => throw new ArgumentException(message);

        public static void ThrowArgumentException(string paramName) => throw new ArgumentException(paramName);

        public static void ThrowArgumentNullException(string paramName) => throw new ArgumentNullException(paramName);

        public static void ThrowNullReferenceException(string message) => throw new NullReferenceException(message);

        public static void ThrowPermissionException(string message) => throw new PermissionException(message);

        public static void ThrowFeatureNotEnabledException(string message) => throw new FeatureNotEnabledException(message);

        public static T ThrowFeatureNotEnabledException<T>(string message) => throw new FeatureNotEnabledException(message);

        public static void ThrowExternalException(string message, int errorCode) => throw new FeatureNotEnabledException(message);

        public static T ThrowExternalException<T>(string message, int errorCode) => throw new FeatureNotEnabledException(message);

        public static T ThrowArgumentNullException<T>(string paramName, string message) => throw new ArgumentNullException(paramName, message);

        public static T ThrowArgumentNullException<T>(string paramName) => throw new ArgumentNullException(paramName);

        public static void ThrowFileNotFoundException(string message, string filePath, Exception innerException) => throw new FileNotFoundException(message, filePath, innerException);

        public static T ThrowFileNotFoundException<T>(string message, string filePath, Exception innerException) => throw new FileNotFoundException(message, filePath, innerException);

        public static void ThrowUnauthorizedAcessException(string message) => throw new UnauthorizedAccessException(message);

        public static T ThrowTaskCancelledException<T>() => throw new TaskCanceledException();

        public static T ThrowException<T>(string message) => throw new Exception(message);
    }
}
