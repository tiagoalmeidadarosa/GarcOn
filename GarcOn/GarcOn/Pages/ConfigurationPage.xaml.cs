using GarcOn.Models;
using GarcOn.NativeDependency;
using GarcOn.Services;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GarcOn.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigurationPage : ContentPage
    {
        public ConfigurationPage()
        {
            InitializeComponent();

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
            DependencyService.Get<IProgressDialog>().LoadingShow();

            if (!string.IsNullOrEmpty(txtIP.Text) && !string.IsNullOrEmpty(txtNumeroMesa.Text))
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
                }
            }
            else
            {
                await DisplayAlert("Não foi possível salvar", "É necessário o preenchimento de todos os campos.", "Cancelar");
            }

            DependencyService.Get<IProgressDialog>().LoadingHide();
        }
    }
}