using System;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Caboodle;
using Xamarin.Forms;
using static System.Math;

namespace Caboodle.Samples.ViewModel
{
    public class MagnetometerViewModel : BaseViewModel
    {
        public MagnetometerViewModel()
        {
            StartCommand = new Command(OnStart);
            StopCommand = new Command(OnStop);
        }

        async void OnStart()
        {
            try
            {
                Magnetometer.Start((SensorSpeed)Speed, (data) =>
                {
                    var change = Pow(data.MagneticFieldX - x, 2.0) + Pow(data.MagneticFieldY - y, 2.0) + Pow(data.MagneticFieldZ - z, 2.0);
                    var text = change > 3000.0 ? "Magnetometer senses something." : "Nothing";
                    switch ((SensorSpeed)Speed)
                    {
                        case SensorSpeed.Fastest:
                        case SensorSpeed.Game:
                            Platform.BeginInvokeOnMainThread(() =>
                            {
                                X = data.MagneticFieldX;
                                Y = data.MagneticFieldY;
                                Z = data.MagneticFieldZ;
                                Info = text;
                            });
                            break;
                        default:
                            X = data.MagneticFieldX;
                            Y = data.MagneticFieldY;
                            Z = data.MagneticFieldZ;
                            Info = text;
                            break;
                    }
                });
                IsActive = true;
            }
            catch (Exception)
            {
                await DisplayAlert("Magnetometer not supported");
            }
        }

        void OnStop()
        {
            IsActive = false;
            Magnetometer.Stop();
        }

        public ICommand StartCommand { get; }

        public ICommand StopCommand { get; }

        double x;

        public double X
        {
            get => x;
            set => SetProperty(ref x, value);
        }

        double y;

        public double Y
        {
            get => y;
            set => SetProperty(ref y, value);
        }

        double z;

        public double Z
        {
            get => z;
            set => SetProperty(ref z, value);
        }

        string info = string.Empty;

        public string Info
        {
            get => info;
            set => SetProperty(ref info, value);
        }

        bool isActive;

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

        int speed = 2;

        public int Speed
        {
            get => speed;
            set => SetProperty(ref speed, value);
        }

        public override void OnDisappearing()
        {
            OnStop();
            base.OnDisappearing();
        }
    }
}
