using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Samples.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactsPage : ContentPage
    {
        public ContactsPage()
        {
            InitializeComponent();
        }

        async void ContatoBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                var p = await Contact.PickContactAsync();

                // var t = p.ToList();
                // await DisplayAlert("Aviso", t.Count.ToString(), "Ok");

                // var euMesmo = await Contact.PickContactAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return;
            }

            // await Contact.SaveContactAsync("PedroTeste", "36819999");
        }
    }
}
