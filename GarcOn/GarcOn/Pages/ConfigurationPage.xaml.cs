using Rg.Plugins.Popup.Services;
using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GarcOn.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigurationPage : ContentPage, INotifyPropertyChanged
    {
        private int _attempt = 0;

        public ConfigurationPage()
        {
            InitializeComponent();

            IsLoading = false;
            BindingContext = this;

            Task.Run(async () =>
            {
                try
                {
                    var ip = await SecureStorage.GetAsync("ip_servidor");
                    var numeroMesa = await SecureStorage.GetAsync("numero_mesa");

                    txtIP.Text = ip;
                    txtNumeroMesa.Text = numeroMesa;
                }
                catch (Exception ex)
                {
                    await DisplayAlert("BUSCA DE VALORES SALVOS", "Não foi possível obter os valores salvos de ip do servidor e número da mesa.", "CANCELAR");
                }

            }).Wait();
        }

        private async void BtnConnect_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtIP.Text) && !string.IsNullOrEmpty(txtNumeroMesa.Text))
            {
                ShowActivityIndicator();

                ServerIsAlive();
            }
            else
            {
                await DisplayAlert("NÃO FOI POSSÍVEL PROSSEGUIR", "É necessário o preenchimento de todos os campos.", "FECHAR");
            }
        }

        private void ServerIsAlive()
        {
            var ipServidor = txtIP.Text;

            var address = new EndpointAddress("http://" + ipServidor + "/GarcOnService");

            var bind = new BasicHttpBinding();
            bind.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            bind.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            var garconClient = new GarcOnClient(bind, address);
            garconClient.ClientCredentials.UserName.UserName = "user";
            garconClient.ClientCredentials.UserName.Password = "password";
            garconClient.GetDataCompleted += GarconClient_GetDataCompleted;
            garconClient.GetDataAsync();
        }

        private async void GarconClient_GetDataCompleted(object sender, GetDataCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if(e.Error.Message.Contains("403"))
                {
                    var ipServidor = txtIP.Text;
                    await SecureStorage.SetAsync("ip_servidor", ipServidor);
                    await SecureStorage.SetAsync("numero_mesa", txtNumeroMesa.Text);

                    HideActivityIndicator();

                    PopupNavigation.Instance.PushAsync(new PasswordPopupPage());
                }
                else
                {
                    if (_attempt < 3)
                    {
                        _attempt++;
                        Thread.Sleep(TimeSpan.FromSeconds(2));

                        ServerIsAlive();
                    }
                    else
                    {
                        _attempt = 0;

                        DisplayAlertOnMainThread("NÃO FOI POSSÍVEL CONECTAR", "Servidor não está respondendo.\n\nErro: " + e.Error.Message, "FECHAR");

                        HideActivityIndicator();
                    }
                }                
            }
        }

        private void DisplayAlertOnMainThread(string title, string message, string cancel)
        {
            Device.BeginInvokeOnMainThread(() => {
                DisplayAlert(title, message, cancel);
            });
        }

        public void ShowActivityIndicator()
        {
            IsLoading = true;
        }

        public void HideActivityIndicator()
        {
            IsLoading = false;
        }

        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }
            set
            {
                this.isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}