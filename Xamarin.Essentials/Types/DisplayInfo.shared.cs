using System;

namespace Xamarin.Essentials
{
    [Preserve(AllMembers = true)]
    public readonly struct DisplayInfo : IEquatable<DisplayInfo>
    {
        public DisplayInfo(double width, double height, double density, DisplayOrientation orientation, DisplayRotation rotation)
        {
            Width = width;
            Height = height;
            Density = density;
            Orientation = orientation;
            Rotation = rotation;
        }

        public double Width { get; }

        public double Height { get; }

        public double Density { get; }

        public DisplayOrientation Orientation { get; }

        public DisplayRotation Rotation { get; }

        public static bool operator ==(DisplayInfo left, DisplayInfo right) =>
            left.Equals(right);

        public static bool operator !=(DisplayInfo left, DisplayInfo right) =>
            !left.Equals(right);

        public override bool Equals(object obj) =>
            (obj is DisplayInfo metrics) && Equals(metrics);

        public bool Equals(DisplayInfo other) =>
            Width.Equals(other.Width) &&
            Height.Equals(other.Height) &&
            Density.Equals(other.Density) &&
            Orientation == other.Orientation &&
            Rotation == other.Rotation;

        public override int GetHashCode() =>
#pragma warning disable CS8356 // Predefined type 'System.ValueTuple`5' is declared in multiple referenced assemblies: 'mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e' and 'System.ValueTuple, Version=4.0.1.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
            (Height, Width, Density, Orientation, Rotation).GetHashCode();
#pragma warning restore CS8356 // Predefined type 'System.ValueTuple`5' is declared in multiple referenced assemblies: 'mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e' and 'System.ValueTuple, Version=4.0.1.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'

        public override string ToString() =>
            $"{nameof(Height)}: {Height}, {nameof(Width)}: {Width}, " +
            $"{nameof(Density)}: {Density}, {nameof(Orientation)}: {Orientation}, " +
            $"{nameof(Rotation)}: {Rotation}";
    }
}
