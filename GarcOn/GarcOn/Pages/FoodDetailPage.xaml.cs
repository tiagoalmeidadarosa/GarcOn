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
        public List<Adicional> AdicionaisSelecionados;

		public FoodDetailPage(Produto produto)
		{
            InitializeComponent();

            NavigationBarView.Title = produto.Nome.ToUpper();
            NavigationBarView.Color = Color.Transparent;

            this.Produto = produto;
            this.AdicionaisSelecionados = new List<Adicional>();

            if (produto.Adicionais.Count > 0)
            {
                lblAdicionais.IsVisible = true;
            }

            foreach (var adicional in produto.Adicionais)
            {
                var checkBox = new CheckBox();
                checkBox.AutomationId = adicional.ID.ToString();
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

        #region Methods

        private void CalculateTotalPrice()
        {
            var valor = Convert.ToDouble(lblPrice.Text.Replace("R$ ", ""));
            var quantidade = Convert.ToInt32(lblQtd.Text);

            double valorTotal = 0;
            foreach (var adicionalSelecionado in AdicionaisSelecionados)
            {
                valorTotal += adicionalSelecionado.Valor * quantidade;
            }

            valorTotal += valor * quantidade;
            lblTotalPrice.Text = string.Format("{0:C}", valorTotal);
        }

        #endregion

        #region Events

        private void CheckBox_CheckChanged(object sender, EventArgs e)
        {
            var checkBox = (CheckBox)sender;

            var adicional = Produto.Adicionais.FirstOrDefault(a => a.ID == Convert.ToInt64(checkBox.AutomationId));
            if(adicional != null)
            {
                if(checkBox.IsChecked)
                {
                    AdicionaisSelecionados.Add(adicional);
                }
                else
                {
                    AdicionaisSelecionados.Remove(adicional);
                }
            }

            CalculateTotalPrice();
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

        private async void AddToBasketButton_Clicked(object sender, EventArgs e)
        {
            var quantidade = Convert.ToInt32(lblQtd.Text);

            var descricao = "Sem adicionais";
            if (AdicionaisSelecionados.Count > 0)
            {
                var adicionais = string.Join(", ", AdicionaisSelecionados.Select(a => a.Descricao));
                descricao = adicionais.Length > 60 ? adicionais.Substring(0, 57) + "..." : adicionais;
            }
            
            var valorTotal = Convert.ToDouble(lblTotalPrice.Text.Replace("R$ ", ""));

            OrderItem orderItem = new OrderItem(Guid.NewGuid(),
                                                Produto.ID,
                                                Produto.Nome,
                                                descricao,
                                                Produto.Valor,
                                                quantidade,
                                                valorTotal,
                                                AdicionaisSelecionados.ToList()
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