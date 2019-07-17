using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static partial class MainThread
    {
        public static bool IsMainThread =>
            PlatformIsMainThread;

        public static void BeginInvokeOnMainThread(Action action)
        {
            if (IsMainThread)
            {
                action();
            }
            else
            {
                PlatformBeginInvokeOnMainThread(action);
            }
        }

        public static Task InvokeOnMainThread(Action action)
        {
            if (IsMainThread)
            {
                action();
#if NETSTANDARD1_0
                return Task.FromResult(true);
#else
                return Task.CompletedTask;
#endif
            }

            var tcs = new TaskCompletionSource<bool>();

            BeginInvokeOnMainThread(() =>
            {
                try
                {
                    action();
                    tcs.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });

            return tcs.Task;
        }

        public static Task<T> InvokeOnMainThread<T>(Func<T> action)
        {
            if (IsMainThread)
            {
                return Task.FromResult(action());
            }

            var tcs = new TaskCompletionSource<T>();

            BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var result = action();
                    tcs.TrySetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });

            return tcs.Task;
        }

        public static Task InvokeOnMainThreadAsync(Action action)
        {
            Func<object> dummyFunc = () =>
            {
                action();
                return null;
            };
            return InvokeOnMainThreadAsync(dummyFunc);
        }

        public static Task<T> InvokeOnMainThreadAsync<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();
            BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var result = func();
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
        }

        public static Task InvokeOnMainThreadAsync(Func<Task> funcTask)
        {
            Func<Task<object>> dummyFunc = () =>
            {
                funcTask();
                return null;
            };

            return InvokeOnMainThreadAsync(dummyFunc);
        }

        public static async Task<SynchronizationContext> GetMainThreadSynchronizationContextAsync()
        {
            SynchronizationContext ret = null;
            await InvokeOnMainThreadAsync(() =>
                ret = SynchronizationContext.Current).ConfigureAwait(false);
            return ret;
        }
    }
}
