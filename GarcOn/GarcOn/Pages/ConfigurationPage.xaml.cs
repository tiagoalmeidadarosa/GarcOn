using GarcOn.Models;
using GarcOn.NativeDependency;
using GarcOn.Services;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GarcOn.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigurationPage : ContentPage, INotifyPropertyChanged
    {
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
                await DisplayAlert("Busca de valores salvos", "Não foi possível obter os valores salvos de ip do servidor e número da mesa.", "Cancelar");
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
            ShowActivityIndicator();

            await Task.Delay(2000);

            if (!string.IsNullOrEmpty(txtIP.Text) && !string.IsNullOrEmpty(txtNumeroMesa.Text))
            {
                try
                {
                    await SecureStorage.SetAsync("ip_servidor", txtIP.Text);
                    await SecureStorage.SetAsync("numero_mesa", txtNumeroMesa.Text);

                    //Buscar atualizações
                    APIService apiService = new APIService(txtIP.Text);
                    var jsonData = apiService.GetData();

                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        App.Categorias = JsonConvert.DeserializeObject<List<Categoria>>(jsonData);
                        await SecureStorage.SetAsync("categorias_e_produtos", jsonData);

                        await DisplayAlert("Atualização concluída", "Informações carregadas com sucesso!", "Fechar");
                        App.Current.MainPage = new MenuPage();
                    }
                }
                catch (Exception ex)
                {
                    if (ex != null)
                    {
                        await DisplayAlert("Não foi possível obter as informações", "Ocorreu um erro: " + ex.Message, "Fechar");
                    }
                }
            }
            else
            {
                await DisplayAlert("Não foi possível salvar", "É necessário o preenchimento de todos os campos.", "Fechar");
            }

            HideActivityIndicator();
        }

        private async void OnJumpAndInitApplicationButton(object sender, EventArgs e)
        {
            if (App.IsAdmin || (!string.IsNullOrEmpty(txtIP.Text) && !string.IsNullOrEmpty(txtNumeroMesa.Text)))
            {
                App.Current.MainPage = new MenuPage();
            }
            else
            {
                await DisplayAlert("Não foi possível prosseguir", "É necessário o preenchimento de todos os campos.", "Fechar");
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