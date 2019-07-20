using GarcOn.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ServiceModel;
using GarcOn.NativeDependency;
using System.Threading;

namespace GarcOn.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RequestAccountPopupPage : PopupPage
    {
        private int _attempt = 0;

        public RequestAccountPopupPage(bool onlyView = false)
		{
			InitializeComponent();

            var itensPedidosFinalizados = App.ItensPedidosFinalizados;

            if (onlyView)
            {
                btnConfirm.IsVisible = false;
                lblSugestao.IsVisible = false;
                editorSugestao.IsVisible = false;
                stkLytSugestao.IsVisible = false;

                lblTitlePage.Text = "VISUALIZAR CONTA";
                itensPedidosFinalizados = App.ItensPedidosFinalizadosUltimaConta;
            }

            for (int i = 0; i < 3; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = GridLength.Auto;
                gridItens.ColumnDefinitions.Add(columnDefinition);
            }

            var count = 0;
            foreach (var item in itensPedidosFinalizados)
            {
                for (int i = 0; i < 3; i++)
                {
                    var label = new Label();
                    label.TextColor = Color.Black;
                    label.HorizontalTextAlignment = TextAlignment.Start;
                    label.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));

                    switch(i)
                    {
                        case 0:
                            label.Text = item.Name;
                            break;
                        case 1:
                            label.HorizontalTextAlignment = TextAlignment.End;
                            label.Text = item.Quantity + " x " + string.Format("{0:C}", item.UnitPrice + item.SelectedAditionals.Sum(a => a.Valor));
                            break;
                        case 2:
                            label.HorizontalTextAlignment = TextAlignment.End;
                            label.Text = string.Format("{0:C}", item.TotalPrice);
                            break;
                    }

                    gridItens.Children.Add(label, i, count);
                }

                count++;
            }

            var labelTotal = new Label();
            labelTotal.HorizontalTextAlignment = TextAlignment.End;
            labelTotal.TextColor = Color.Black;
            labelTotal.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            labelTotal.Text = "Total: ";
            gridItens.Children.Add(labelTotal, 1, count + 1);

            var labelValorTotal = new Label();
            labelValorTotal.HorizontalTextAlignment = TextAlignment.End;
            labelValorTotal.TextColor = Color.Black;
            labelValorTotal.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            labelValorTotal.Text = string.Format("{0:C}", itensPedidosFinalizados.Sum(i => i.TotalPrice));
            gridItens.Children.Add(labelValorTotal, 2, count + 1);
        }

        protected async override void OnAppearing()
        {
            lblConsumosMesa.Text = "Consumos da mesa " + await SecureStorage.GetAsync("numero_mesa") + ":";
        }

        private async void CloseImage_TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            var completeAccount = await DisplayAlert("FECHAR CONTA", "Deseja realmente solicitar a conta?", "SIM", "NÃO");

            if (completeAccount)
            {
                //Executa loading
                DependencyService.Get<IProgressDialog>().LoadingShow();

                AddAccountRequest();
            }
        }

        private async void AddAccountRequest()
        {
            var ipServidor = await SecureStorage.GetAsync("ip_servidor");
            var numeroMesa = Convert.ToInt32(await SecureStorage.GetAsync("numero_mesa"));
            var valorTotal = Convert.ToDouble(App.ItensPedidosFinalizados.Sum(i => i.TotalPrice));
            var sugestao = editorSugestao.Text;

            var address = new EndpointAddress("http://" + ipServidor + "/GarcOnService");

            var bind = new BasicHttpBinding();
            bind.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            bind.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            var garconClient = new GarcOnClient(bind, address);
            garconClient.ClientCredentials.UserName.UserName = "admin";
            garconClient.ClientCredentials.UserName.Password = "admin";
            garconClient.AddAccountRequestCompleted += GarconClient_AddAccountRequestCompleted;
            garconClient.AddAccountRequestAsync(numeroMesa, valorTotal, sugestao);
        }

        private async void GarconClient_AddAccountRequestCompleted(object sender, AddAccountRequestCompletedEventArgs e)
        {
            var errorMessage = "";
            if (e.Error == null)
                errorMessage = e.Result;
            else
                errorMessage = e.Error.Message;

            if (string.IsNullOrEmpty(errorMessage))
            {
                _attempt = 0;

                App.ItensPedidosFinalizadosUltimaConta = App.ItensPedidosFinalizados;
                App.ItensPedidosFinalizados = new List<OrderItem>();

                DisplayAlertOnMainThread("CONFIRMAÇÃO DE FECHAMENTO DE CONTA", "Sua solicitação foi cadastrada com sucesso, aguarde um momento que alguém irá atendê-lo. :)", "FECHAR");

                HideProgressDialogOnMainThread();

                await PopupNavigation.Instance.PopAsync(true);

                App.Current.MainPage = new MenuPage();
            }
            else
            {
                if (_attempt < 3)
                {
                    _attempt++;
                    Thread.Sleep(TimeSpan.FromSeconds(2));

                    AddAccountRequest();
                }
                else
                {
                    _attempt = 0;

                    DisplayAlertOnMainThread("ERRO NO FECHAMENTO DA CONTA", "Não foi possível cadastrar a solicitação, talvez o servidor não esteja respondendo, tente novamente em alguns instantes. Erro: " + errorMessage, "FECHAR");

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