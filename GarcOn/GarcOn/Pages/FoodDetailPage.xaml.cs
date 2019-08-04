using GarcOn.Models;
using GarcOn.ViewModels;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
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

            lblTitle.Text = produto.Nome;
            lblPrice.Text = string.Format("{0:C}", produto.Valor);
            lblDescription.Text = produto.Descricao;

            lblQtd.Text = MinValue.ToString();
            lblTotalPrice.Text = string.Format("{0:C}", produto.Valor * MinValue);

            if(produto.Adicionais.Count > 0)
            {
                btnSelectAditional.IsVisible = true;
                btnAddToBasket.IsVisible = false;
            }
            else
            {
                btnSelectAditional.IsVisible = false;
                btnAddToBasket.IsVisible = true;
            }
        }

        protected override void OnAppearing()
        {
            NavigationBarView.BasketValue = App.ItensPedido.Count.ToString();
        }

        #region Methods

        private void CalculateTotalPrice()
        {
            var valor = Convert.ToDouble(lblPrice.Text.Replace("R$ ", ""));
            var quantidade = Convert.ToInt32(lblQtd.Text);

            double valorTotal = valor * quantidade;
            lblTotalPrice.Text = string.Format("{0:C}", valorTotal);
        }

        #endregion

        #region Events

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

                    CalculateTotalPrice();
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

                    CalculateTotalPrice();
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
        }

        private async void BtnSelectAditional_Clicked(object sender, EventArgs e)
        {
            var quantidade = Convert.ToInt32(lblQtd.Text);

            await Navigation.PushAsync(new FoodAditionalPage(Produto, quantidade));
        }

        private async void BtnAddToBasket_Clicked(object sender, EventArgs e)
        {
            var descricao = "Sem adicionais";
            var quantidade = Convert.ToInt32(lblQtd.Text);         
            var valorTotal = Convert.ToDouble(lblTotalPrice.Text.Replace("R$ ", ""));
            var imageDownArrow = quantidade == 1 ? "remove.png" : "arrow_down.png";

            OrderItem orderItem = new OrderItem(Guid.NewGuid(),
                                                Produto.ID,
                                                Produto.Nome,
                                                descricao,
                                                Produto.Valor,
                                                quantidade,
                                                valorTotal,
                                                new List<Adicional>(),
                                                imageDownArrow
                                                //, Todo: Adicionar também a foto
                                                );

            App.ItensPedido.Add(orderItem);

            await Navigation.PushAsync(new BasketPage());
        }

        private async void BadgeToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BasketPage());
        }

        #endregion
    }
}