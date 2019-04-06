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
	public partial class ConfigurationPage : ContentPage
	{
		public ConfigurationPage ()
		{
			InitializeComponent();

            MessagingCenter.Subscribe<PasswordPopupPage>(this, "Refresh", (s) => {
                RefreshProperties();
            });

            PopupNavigation.Instance.PushAsync(new PasswordPopupPage());
        }

        private void RefreshProperties()
        {
            if (App._isAdmin)
            {
                txtIP.IsEnabled = true;
                btnAtualizacoes.IsEnabled = true;
            }
            else
            {
                txtIP.IsEnabled = false;
                btnAtualizacoes.IsEnabled = false;
            }
        }
    }
}