using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class GyroscopeViewModel : BaseViewModel
    {
        private double x;
        private double y;
        private double z;
        private bool isActive;
        private int speed = 2;

        public GyroscopeViewModel()
        {
            StartCommand = new Command(OnStart);
            StopCommand = new Command(OnStop);

            Gyroscope.ReadingChanged += OnReadingChanged;
        }

        public ICommand StartCommand { get; }

        public ICommand StopCommand { get; }

        public double X
        {
            get => x;
            set => SetProperty(ref x, value);
        }

        public double Y
        {
            get => y;
            set => SetProperty(ref y, value);
        }

        public double Z
        {
            get => z;
            set => SetProperty(ref z, value);
        }

        public bool IsActive
        {
            get => isActive;
            set => SetProperty(ref isActive, value);
        }

        public List<string> Speeds { get; } =
           new List<string>
           {
                "Fastest",
                "Game",
                "Normal",
                "User Interface"
           };

        public int Speed
        {
            get => speed;
            set => SetProperty(ref speed, value);
        }

        public override void OnDisappearing()
        {
            OnStop();
            Gyroscope.ReadingChanged -= OnReadingChanged;

            base.OnDisappearing();
        }

        private void OnReadingChanged(GyroscopeChangedEventArgs e)
        {
            var data = e.Reading;
            switch ((SensorSpeed)Speed)
            {
                case SensorSpeed.Fastest:
                case SensorSpeed.Game:
                    Platform.BeginInvokeOnMainThread(() =>
                    {
                        X = data.AngularVelocityX;
                        Y = data.AngularVelocityY;
                        Z = data.AngularVelocityZ;
                    });
                    break;
                default:
                    X = data.AngularVelocityX;
                    Y = data.AngularVelocityY;
                    Z = data.AngularVelocityZ;
                    break;
            }
        }

        private async void OnStart()
        {
            try
            {
                Gyroscope.Start((SensorSpeed)Speed);
                IsActive = true;
            }
            catch (Exception)
            {
                await DisplayAlert("Gyroscope not supported");
            }
        }

        private void OnStop()
        {
            IsActive = false;
            Gyroscope.Stop();
        }
    }
}
