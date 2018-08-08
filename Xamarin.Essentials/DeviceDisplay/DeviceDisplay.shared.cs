using System;

namespace Xamarin.Essentials
{
    public static partial class DeviceDisplay
    {
        static event EventHandler<ScreenMetricsChangedEventArgs> ScreenMetricsChangedInternal;

        static ScreenMetrics currentMetrics;

        public static ScreenMetrics ScreenMetrics => GetScreenMetrics();

        static void SetCurrent()
        {
            var metrics = GetScreenMetrics();
            currentMetrics = new ScreenMetrics(metrics.Width, metrics.Height, metrics.Density, metrics.Orientation, metrics.Rotation);
        }

        public static event EventHandler<ScreenMetricsChangedEventArgs> ScreenMetricsChanged
        {
            add
            {
                var wasRunning = ScreenMetricsChangedInternal != null;

                ScreenMetricsChangedInternal += value;

                if (!wasRunning && ScreenMetricsChangedInternal != null)
                {
                    SetCurrent();
                    StartScreenMetricsListeners();
                }
            }

            remove
            {
                var wasRunning = ScreenMetricsChangedInternal != null;

                ScreenMetricsChangedInternal -= value;

                if (wasRunning && ScreenMetricsChangedInternal == null)
                    StopScreenMetricsListeners();
            }
        }

        static void OnScreenMetricsChanged(ScreenMetrics metrics)
            => OnScreenMetricsChanged(new ScreenMetricsChangedEventArgs(metrics));

        static void OnScreenMetricsChanged(ScreenMetricsChangedEventArgs e)
        {
            if (e.Metrics.Width != currentMetrics.Width ||
                e.Metrics.Height != currentMetrics.Height ||
                e.Metrics.Density != currentMetrics.Density ||
                e.Metrics.Orientation != currentMetrics.Orientation ||
                e.Metrics.Rotation != currentMetrics.Rotation)
            {
                SetCurrent();
                ScreenMetricsChangedInternal?.Invoke(null, e);
            }
        }
    }

    public class ScreenMetricsChangedEventArgs : EventArgs
    {
        public ScreenMetricsChangedEventArgs(ScreenMetrics metrics)
        {
            Metrics = metrics;
        }

        public ScreenMetrics Metrics { get; }
    }

    [Preserve(AllMembers = true)]
    public struct ScreenMetrics
    {
        internal ScreenMetrics(double width, double height, double density, ScreenOrientation orientation, ScreenRotation rotation)
        {
            Width = width;
            Height = height;
            Density = density;
            Orientation = orientation;
            Rotation = rotation;
        }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Density { get; set; }

        public ScreenOrientation Orientation { get; set; }

        public ScreenRotation Rotation { get; set; }
    }

    public enum ScreenOrientation
    {
        Unknown,

        Portrait,
        Landscape
    }

    public enum ScreenRotation
    {
        Rotation0,
        Rotation90,
        Rotation180,
        Rotation270
    }
}
