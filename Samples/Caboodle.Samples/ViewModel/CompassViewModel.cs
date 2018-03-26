using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Microsoft.Caboodle;
using Xamarin.Forms;

namespace Caboodle.Samples.ViewModel
{
    class CompassViewModel : BaseViewModel
    {
        public CompassViewModel()
        {
            StartCompass1Command = new Command(async () =>
            {
                try
                {
                    token1 = new CancellationTokenSource();
                    Compass.Monitor((SensorSpeed)Speed1, token1.Token, (degrees) =>
                    {
                        switch ((SensorSpeed)Speed1)
                        {
                            case SensorSpeed.Fastest:
                            case SensorSpeed.Game:
                                Platform.BeginInvokeOnMainThread(() => { Compass1 = degrees; });
                                break;
                            default:
                                Compass1 = degrees;
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
                token1?.Cancel();
                token1?.Dispose();
                token1 = null;
            });

            StartCompass2Command = new Command(async () =>
            {
                try
                {
                    token2 = new CancellationTokenSource();
                    Compass.Monitor((SensorSpeed)Speed2, token2.Token, (degrees) =>
                    {
                        switch ((SensorSpeed)Speed2)
                        {
                            case SensorSpeed.Fastest:
                            case SensorSpeed.Game:
                                Platform.BeginInvokeOnMainThread(() => { Compass2 = degrees; });
                                break;
                            default:
                                Compass2 = degrees;
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
                token2?.Cancel();
                token2?.Dispose();
                token2 = null;
            });
        }

        CancellationTokenSource token1;
        CancellationTokenSource token2;

        public ICommand StartCompass1Command { get; }

        public ICommand StopCompass1Command { get; }

        public ICommand StartCompass2Command { get; }

        public ICommand StopCompass2Command { get; }

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
