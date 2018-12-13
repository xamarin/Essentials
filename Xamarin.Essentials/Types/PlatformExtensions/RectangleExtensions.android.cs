using System;
using System.Drawing;
using Android.Graphics;

namespace Xamarin.Essentials
{
    public static class RectangleExtensions
    {
        public static Rectangle ToRectangle(this Rect rect) =>
            new Rectangle(rect.Left, rect.Top, rect.Width(), rect.Height());

        public static Rect ToRect(this Rectangle rect) =>
            new Rect(rect.Left, rect.Top, rect.Right, rect.Bottom);
    }
}
