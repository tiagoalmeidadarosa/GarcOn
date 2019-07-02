using GarcOn.Models;
using Plugin.InputKit.Shared.Controls;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Plugin.InputKit.Shared.Controls.CheckBox;

namespace GarcOn.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FoodDetailPage : ContentPage
	{
        const int MinValue = 1;
        const int MaxValue = 50;

        public Produto Produto;

		public FoodDetailPage(Produto produto)
		{
            InitializeComponent();

            NavigationBarView.Title = produto.Nome.ToUpper();
            NavigationBarView.Color = Color.Transparent;

            this.Produto = produto;

            foreach (var adicional in produto.Adicionais)
            {
                var checkBox = new CheckBox();
                checkBox.Type = CheckType.Check;
                checkBox.Color = Color.Red;
                checkBox.Text = adicional.Descricao + " (+ " + string.Format("{0:C}", adicional.Valor) + ")";
                checkBox.Key = Convert.ToInt32(adicional.ID);
                checkBox.CheckChanged += CheckBox_CheckChanged;

                stkLytAdicionais.Children.Add(checkBox);
            }

            lblTitle.Text = produto.Nome;
            lblPrice.Text = string.Format("{0:C}", produto.Valor);
            lblDescription.Text = produto.Descricao;

            lblQtd.Text = MinValue.ToString();
            lblTotalPrice.Text = string.Format("{0:C}", produto.Valor * MinValue);
        }

        protected override void OnAppearing()
        {
            NavigationBarView.BasketValue = App.ItensPedido.Count.ToString();
        }

        private void CheckBox_CheckChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private async void PlusButton_OnClicked(object sender, EventArgs e)
        {
            var quantidade = Convert.ToInt32(lblQtd.Text);
            quantidade++;

            if (quantidade <= MaxValue)
            {
                try
                {
                    await lblQtd.ScaleTo(1.3, 100);
                    lblQtd.Text = quantidade.ToString();
                    await lblQtd.ScaleTo(1, 100);

                    var valor = Convert.ToDouble(lblPrice.Text.Replace("R$ ", ""));
                    lblTotalPrice.Text = string.Format("{0:C}", valor * quantidade);
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
        }

        private async void MinusButton_OnClicked(object sender, EventArgs e)
        {
            var quantidade = Convert.ToInt32(lblQtd.Text);
            quantidade--;

            if (quantidade >= MinValue)
            {
                try
                {
                    await lblQtd.ScaleTo(1.3, 100);
                    lblQtd.Text = quantidade.ToString();
                    await lblQtd.ScaleTo(1, 100);

                    var valor = Convert.ToDouble(lblPrice.Text.Replace("R$ ", ""));
                    lblTotalPrice.Text = string.Format("{0:C}", valor * quantidade);
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
        }

        private async void AddToBasketButton_Clicked(object sender, EventArgs e)
        {
            var quantidade = Convert.ToInt32(lblQtd.Text);

            if (App.ItensPedido.ContainsKey(Produto))
            {
                quantidade += App.ItensPedido[Produto];
                App.ItensPedido.Remove(Produto);
            }

            App.ItensPedido.Add(Produto, quantidade);

            await Navigation.PushAsync(new BasketPage());
        }

        private async void BadgeToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BasketPage());
        }
    }
}