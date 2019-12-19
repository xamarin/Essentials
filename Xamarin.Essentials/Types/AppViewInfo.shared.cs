using System;

namespace Xamarin.Essentials
{
    public readonly struct AppViewInfo : IEquatable<AppViewInfo>
    {
        public AppViewInfo(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public double Width { get; }

        public double Height { get; }

        public bool Equals(AppViewInfo other) =>
            Width.Equals(other.Width) && Height.Equals(other.Height);

        public override bool Equals(object obj) =>
            (obj is AppViewInfo windowSize) && Equals(windowSize);

        public override int GetHashCode() =>
            (Width, Height).GetHashCode();

        public override string ToString() =>
            $"{nameof(Width)} : {Width} \n {nameof(Height)}:{Height}";

        public static bool operator ==(AppViewInfo left, AppViewInfo right) =>
            Equals(left, right);

        public static bool operator !=(AppViewInfo left, AppViewInfo right) =>
            !Equals(left, right);
    }
}
