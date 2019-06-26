using GarcOn.Models;
using GarcOn.Services;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GarcOn.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RequestAccountPopupPage : PopupPage
    {
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
                var produto = item.Key;
                var quantidade = item.Value;

                for (int i = 0; i < 3; i++)
                {
                    var label = new Label();
                    label.TextColor = Color.Black;
                    label.HorizontalTextAlignment = TextAlignment.Start;
                    label.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));

                    switch(i)
                    {
                        case 0:
                            label.Text = produto.Nome;
                            break;
                        case 1:
                            label.HorizontalTextAlignment = TextAlignment.End;
                            label.Text = quantidade + " x " + string.Format("{0:C}", produto.Valor);
                            break;
                        case 2:
                            label.HorizontalTextAlignment = TextAlignment.End;
                            label.Text = string.Format("{0:C}", quantidade * produto.Valor);
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
            labelValorTotal.Text = string.Format("{0:C}", itensPedidosFinalizados.Sum(i => i.Value * i.Key.Valor));
            gridItens.Children.Add(labelValorTotal, 2, count + 1);
        }

        protected async override void OnAppearing()
        {
            try
            {
                lblConsumosMesa.Text = "Consumos da mesa " + await SecureStorage.GetAsync("numero_mesa") + ":";
            }
            catch(Exception ex)
            {

            }
        }

        private async void CloseImage_TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            var completeAccount = await DisplayAlert("Fechar conta", "Deseja realmente solicitar a conta?", "Sim", "Não");

            if (completeAccount)
            {
                var ip = await SecureStorage.GetAsync("ip_servidor");
                var numeroMesa = Convert.ToInt32(await SecureStorage.GetAsync("numero_mesa"));
                var valorTotal = Convert.ToDouble(App.ItensPedidosFinalizados.Sum(i => i.Value * i.Key.Valor));
                var sugestao = editorSugestao.Text;

                APIService apiService = new APIService(ip);
                var errorMessage = apiService.AddAccountRequest(numeroMesa, valorTotal, sugestao);

                if (string.IsNullOrEmpty(errorMessage))
                {
                    await DisplayAlert("Confirmação de fechamento de conta", "Sua solicitação foi cadastrada com sucesso, aguarde um momento que alguém irá atendê-lo. :)", "Fechar");

                    App.ItensPedidosFinalizadosUltimaConta = App.ItensPedidosFinalizados;

                    App.ItensPedidosFinalizados = new Dictionary<Produto, int>();
                    App.Current.MainPage = new MenuPage();

                    await PopupNavigation.Instance.PopAsync(true);
                }
                else
                {
                    await DisplayAlert("Erro no fechamento da conta", "Não foi possível cadastrar a solicitação, talvez o servidor não esteja respondendo, tente novamente em alguns instantes. Erro: " + errorMessage, "Fechar");
                }
            }
        }
    }
}