using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GarcOn.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PasswordPopupPage : PopupPage
	{
		public PasswordPopupPage()
		{
			InitializeComponent();
		}

        private async void OnLoginButton(object sender, EventArgs e)
        {
            if(txtPassword.Text == "admin")
            {
                App.IsAdmin = true;
                MessagingCenter.Send(this, "Refresh");
                await PopupNavigation.Instance.PopAsync(true);
            }
            else
            {
                await DisplayAlert("NÃO FOI POSSÍVEL CONECTAR", "Senha incorreta!", "CANCELAR");
            }
        }

        private async void OnCancelButton(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}