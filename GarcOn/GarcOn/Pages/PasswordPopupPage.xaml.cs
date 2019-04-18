using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GarcOn.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PasswordPopupPage : PopupPage
	{
		public PasswordPopupPage ()
		{
			InitializeComponent ();
		}

        private async void OnLoginButton(object sender, EventArgs e)
        {
            if(txtPassword.Text == "GarcOnPass123")
            {
                App.IsAdmin = true;
                MessagingCenter.Send<PasswordPopupPage>(this, "Refresh");
                await PopupNavigation.Instance.PopAsync(true);
            }
            else
            {
                await DisplayAlert("Não foi possível conectar", "Senha incorreta!", "Cancelar");
            }
        }

        private async void OnBackButton(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}