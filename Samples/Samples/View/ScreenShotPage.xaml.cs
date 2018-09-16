using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Samples.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Samples.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScreenShotPage : ContentPage
    {
        public ScreenShotPage()
        {
            InitializeComponent();
            BindingContext = new ScreenShotViewModel();
        }
    }
}
