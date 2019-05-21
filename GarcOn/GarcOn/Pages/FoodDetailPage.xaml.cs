using GarcOn.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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