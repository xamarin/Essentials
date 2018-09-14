using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Samples.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScreenPage : ContentPage
    {
        public ScreenPage()
        {
            InitializeComponent();
            BindingContext = new ViewModel.ScreenViewModel();
        }
    }
}
