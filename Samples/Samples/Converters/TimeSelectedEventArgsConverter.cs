using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Samples.Converters
{
    public class TimeSelectedEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as PropertyChangedEventArgs;

            if (eventArgs == null || eventArgs.PropertyName != nameof(TimePicker.Time))
                return null;

            return eventArgs.PropertyName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
