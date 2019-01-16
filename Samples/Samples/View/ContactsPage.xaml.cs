using System;
using System.Collections.Generic;
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
            var contact = await Contact.PickContact();
            ContatoResult.Text = contact.Name + contact.Emails.FirstOrDefault();
        }
    }
}
