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
                txtIP.Text = await SecureStorage.GetAsync("ip_servidor");
                txtNumeroMesa.Text = await SecureStorage.GetAsync("numero_mesa");
            }
            catch (Exception ex) { }
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
                    var jsonCategoriesAndProducts = await apiService.GetCategoriesAndProducts();

                    if (!string.IsNullOrEmpty(jsonCategoriesAndProducts))
                    {
                        App.Categorias = JsonConvert.DeserializeObject<List<Categoria>>(jsonCategoriesAndProducts);
                        await SecureStorage.SetAsync("categorias_e_produtos", jsonCategoriesAndProducts);

                        await DisplayAlert("Atualização concluída", "Informações carregadas com sucesso!", "Cancelar");
                    }
                }
                catch (Exception ex)
                {
                    if (ex != null)
                    {
                        await DisplayAlert("Não foi possível obter as informações", "Ocorreu um erro: " + ex.Message, "Cancelar");
                    }
                }
            }
            else
            {
                await DisplayAlert("Não foi possível salvar", "É necessário o preenchimento de todos os campos.", "Cancelar");
            }

            HideActivityIndicator();
        }

        public async void ShowActivityIndicator()
        {
            IsLoading = true;
        }

        public async void HideActivityIndicator()
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