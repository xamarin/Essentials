﻿using System;
using System.Drawing;
using AppKit;

namespace Xamarin.Essentials
{
    public static partial class ColorExtensions
    {
        public static Color ToSystemColor(this NSColor color)
        {
            if (color == null)
                throw new ArgumentNullException(nameof(color));

            // make sure the colorspace is valid for RGBA
            // we can't check as the check will throw if it is invalid
            color = color.UsingColorSpace(NSColorSpace.SRGBColorSpace);

            color.GetRgba(out var red, out var green, out var blue, out var alpha);
            return Color.FromArgb((int)Math.Round(alpha * 255), (int)Math.Round(red * 255), (int)Math.Round(green * 255), (int)Math.Round(blue * 255));
        }

        public static NSColor ToPlatformColor(this Color color) =>
            NSColor.FromRgba(color.R, color.G, color.B, color.A);
    }
}
