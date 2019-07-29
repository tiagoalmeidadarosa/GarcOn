using GarcOn.Models;
using GarcOn.NativeDependency;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GarcOn.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PasswordPopupPage : PopupPage
	{
        private int _attempt = 0;

        public PasswordPopupPage()
		{
			InitializeComponent();
		}

        private async void BtnInitApplication_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUser.Text) && !string.IsNullOrEmpty(txtPassword.Text))
            {
                //Executa loading
                DependencyService.Get<IProgressDialog>().LoadingShow();

                GetData();
            }
            else
            {
                await DisplayAlert("NÃO FOI POSSÍVEL PROSSEGUIR", "É necessário o preenchimento de todos os campos.", "FECHAR");
            }
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void GetData()
        {
            var ipServidor = await SecureStorage.GetAsync("ip_servidor");

            var user = txtUser.Text;
            var password = txtPassword.Text;

            var address = new EndpointAddress("http://" + ipServidor + "/GarcOnService");

            var bind = new BasicHttpBinding();
            bind.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            bind.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            var garconClient = new GarcOnClient(bind, address);
            garconClient.ClientCredentials.UserName.UserName = user;
            garconClient.ClientCredentials.UserName.Password = password;
            garconClient.GetDataCompleted += GarconClient_GetDataCompleted;
            garconClient.GetDataAsync();
        }

        private async void GarconClient_GetDataCompleted(object sender, GetDataCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                _attempt = 0;

                App.User = txtUser.Text;
                App.Password = txtPassword.Text;

                var jsonData = e.Result;

                if (!string.IsNullOrEmpty(jsonData))
                {
                    App.Categorias = JsonConvert.DeserializeObject<List<Categoria>>(jsonData);

                    DisplayAlertOnMainThread("ATUALIZAÇÃO CONCLUÍDA", "Informações carregadas com sucesso!", "FECHAR");

                    await PopupNavigation.Instance.PopAsync(true);

                    App.Current.MainPage = new MenuPage();
                }

                HideProgressDialogOnMainThread();
            }
            else
            {
                if (_attempt < 3)
                {
                    _attempt++;
                    Thread.Sleep(TimeSpan.FromSeconds(2));

                    GetData();
                }
                else
                {
                    _attempt = 0;

                    DisplayAlertOnMainThread("NÃO FOI POSSÍVEL OBTER AS INFORMAÇÕES", "Usuário ou senha incorretos.\n\nErro: " + e.Error.Message, "FECHAR");

                    HideProgressDialogOnMainThread();
                }
            }
        }

        private void DisplayAlertOnMainThread(string title, string message, string cancel)
        {
            Device.BeginInvokeOnMainThread(() => {
                DisplayAlert(title, message, cancel);
            });
        }

        private void HideProgressDialogOnMainThread()
        {
            Device.BeginInvokeOnMainThread(() => {
                DependencyService.Get<IProgressDialog>().LoadingHide();
            });
        }
    }
}