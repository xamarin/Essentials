using Caboodle.Samples.ViewModel;
using Microsoft.Caboodle;
using Xamarin.Forms;

namespace Caboodle.Samples.View
{
    public partial class FlashlightPage : ContentPage
    {
        public FlashlightPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Battery.BatteryChanged += OnBatteryChanged;
        }

        protected override void OnDisappearing()
        {
            Battery.BatteryChanged -= OnBatteryChanged;

            base.OnDisappearing();
        }

        void OnBatteryChanged(BatteryChangedEventArgs e)
        {
            if (BindingContext is BatteryViewModel vm)
            {
                vm.Update(e);
            }
        }
    }
}
