using System;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Caboodle;
using Xamarin.Forms;

namespace Caboodle.Samples.ViewModel
{
    class CompassViewModel : BaseViewModel
    {
        public CompassViewModel()
        {
            Init();
        }

        void Init()
        {
            StartCompass1Command = new Command(async () =>
            {
                try
                {
                    if (Compass.IsMonitoring)
                        StopCompass2Command.Execute(null);

                    Compass.Start((SensorSpeed)Speed1, (data) =>
                    {
                        switch ((SensorSpeed)Speed1)
                        {
                            case SensorSpeed.Fastest:
                            case SensorSpeed.Game:
                                Platform.BeginInvokeOnMainThread(() => { Compass1 = data.HeadingMagneticNorth; });
                                break;
                            default:
                                Compass1 = data.HeadingMagneticNorth;
                                break;
                        }
                    });
                    Compass1IsActive = true;
                }
                catch (Exception)
                {
                    await DisplayAlert("Compass not supported");
                }
            });

            StopCompass1Command = new Command(() =>
            {
                Compass1IsActive = false;
                Compass.Stop();
            });

            StartCompass2Command = new Command(async () =>
            {
                try
                {
                    if (Compass.IsMonitoring)
                        StopCompass1Command.Execute(null);

                    Compass.Start((SensorSpeed)Speed2, (data) =>
                    {
                        switch ((SensorSpeed)Speed2)
                        {
                            case SensorSpeed.Fastest:
                            case SensorSpeed.Game:
                                Platform.BeginInvokeOnMainThread(() => { Compass2 = data.HeadingMagneticNorth; });
                                break;
                            default:
                                Compass2 = data.HeadingMagneticNorth;
                                break;
                        }
                    });
                    Compass2IsActive = true;
                }
                catch (Exception)
                {
                    await DisplayAlert("Compass not supported");
                }
            });

            StopCompass2Command = new Command(() =>
            {
                Compass2IsActive = false;
                Compass.Stop();
            });
        }

        public ICommand StartCompass1Command { get; private set; }

        public ICommand StopCompass1Command { get; private set; }

        public ICommand StartCompass2Command { get; private set; }

        public ICommand StopCompass2Command { get; private set; }

        bool compass1IsActive;

        public bool Compass1IsActive
        {
            get => compass1IsActive;
            set => SetProperty(ref compass1IsActive, value);
        }

        bool compass2IsActive;

        public bool Compass2IsActive
        {
            get => compass2IsActive;
            set => SetProperty(ref compass2IsActive, value);
        }

        double compass1;

        public double Compass1
        {
            get => compass1;
            set => SetProperty(ref compass1, value);
        }

        double compass2;

        public double Compass2
        {
            get => compass2;
            set => SetProperty(ref compass2, value);
        }

        int speed1 = 2;

        public int Speed1
        {
            get => speed1;
            set => SetProperty(ref speed1, value);
        }

        int speed2 = 2;

        public int Speed2
        {
            get => speed2;
            set => SetProperty(ref speed2, value);
        }

        public List<string> CompassSpeeds { get; } =
            new List<string>
            {
                "Fastest",
                "Game",
                "Normal",
                "User Interface"
            };

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            StopCompass1Command.Execute(null);
            StopCompass2Command.Execute(null);
        }
    }
}
