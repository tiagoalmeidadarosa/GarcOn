using GarcOn.Models;
using GarcOn.NativeDependency;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.Threading;
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

            MessagingCenter.Subscribe<PasswordPopupPage>(this, "Refresh", (s) => {
                RefreshProperties();
            });

            PopupNavigation.Instance.PushAsync(new PasswordPopupPage());
        }

        protected override async void OnAppearing()
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
        }

        private void RefreshProperties()
        {
            if (App.IsAdmin)
            {
                txtIP.IsEnabled = true;
                txtNumeroMesa.IsEnabled = true;
                btnAtualizacoes.IsEnabled = true;
            }
            else
            {
                txtIP.IsEnabled = false;
                txtNumeroMesa.IsEnabled = false;
                btnAtualizacoes.IsEnabled = false;
            }
        }

        private async void OnSaveAndGetUpdate(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtIP.Text) && !string.IsNullOrEmpty(txtNumeroMesa.Text))
            {
                ShowActivityIndicator();

                var ipServidor = txtIP.Text;
                await SecureStorage.SetAsync("ip_servidor", ipServidor);
                await SecureStorage.SetAsync("numero_mesa", txtNumeroMesa.Text);

                //Buscar atualizações de categorias e produtos
                GetData();
            }
            else
            {
                await DisplayAlert("NÃO FOI POSSÍVEL SALVAR", "É necessário o preenchimento de todos os campos.", "FECHAR");
            }
        }

        private void GetData()
        {
            var ipServidor = txtIP.Text;

            var address = new EndpointAddress("http://" + ipServidor + "/GarcOnService");
            BasicHttpBinding bind = new BasicHttpBinding();
            
            var garconClient = new GarcOnClient(bind, address);
            garconClient.GetDataCompleted += GarconClient_GetDataCompleted;
            garconClient.GetDataAsync();
        }

        private async void GarconClient_GetDataCompleted(object sender, GetDataCompletedEventArgs e)
        {
            if(e.Error == null)
            {
                _attempt = 0;

                var jsonData = e.Result;

                if (!string.IsNullOrEmpty(jsonData))
                {
                    App.Categorias = JsonConvert.DeserializeObject<List<Categoria>>(jsonData);
                    await SecureStorage.SetAsync("categorias_e_produtos", jsonData);

                    DisplayAlertOnMainThread("ATUALIZAÇÃO CONCLUÍDA", "Informações carregadas com sucesso!", "FECHAR");

                    App.Current.MainPage = new MenuPage();
                }

                HideActivityIndicator();
            }
            else
            {
                if(_attempt < 3)
                {
                    _attempt++;
                    Thread.Sleep(TimeSpan.FromSeconds(2));

                    GetData();
                }
                else
                {
                    _attempt = 0;

                    DisplayAlertOnMainThread("NÃO FOI POSSÍVEL OBTER AS INFORMAÇÕES", "Ocorreu um erro: " + e.Error.Message, "FECHAR");

                    HideActivityIndicator();
                }
            }
        }

        private void DisplayAlertOnMainThread(string title, string message, string cancel)
        {
            Device.BeginInvokeOnMainThread(() => {
                DisplayAlert(title, message, cancel);
            });
        }

        private async void OnJumpAndInitApplicationButton(object sender, EventArgs e)
        {
            if (App.IsAdmin || (!string.IsNullOrEmpty(txtIP.Text) && !string.IsNullOrEmpty(txtNumeroMesa.Text)))
            {
                App.Current.MainPage = new MenuPage();
            }
            else
            {
                await DisplayAlert("NÃO FOI POSSÍVEL PROSSEGUIR", "É necessário o preenchimento de todos os campos.", "FECHAR");
            }
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