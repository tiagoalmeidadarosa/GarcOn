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
	public partial class FoodAditionalPage : ContentPage
	{
        public Produto Produto;
        public int Quantidade;
        public List<Adicional> AdicionaisSelecionados;

		public FoodAditionalPage(Produto produto, int quantidade)
		{
            InitializeComponent();

            NavigationBarView.Title = "SELECIONAR ADICIONAIS";

            this.Produto = produto;
            this.Quantidade = quantidade;
            this.AdicionaisSelecionados = new List<Adicional>();

            foreach (var adicional in produto.Adicionais)
            {
                var checkBox = new CheckBox();
                checkBox.BoxSizeRequest = 22;
                checkBox.AutomationId = adicional.ID.ToString();
                checkBox.Type = CheckType.Check;
                checkBox.Color = Color.Red;
                checkBox.Text = adicional.Descricao + " (+ " + string.Format("{0:C}", adicional.Valor) + ")";
                checkBox.TextFontSize = 22;
                checkBox.Key = Convert.ToInt32(adicional.ID);
                checkBox.CheckChanged += CheckBox_CheckChanged;

                stkLytAdicionais.Children.Add(checkBox);
            }

            lblTotalPrice.Text = string.Format("{0:C}", produto.Valor * quantidade);
        }

        protected override void OnAppearing()
        {
            NavigationBarView.BasketValue = App.ItensPedido.Count.ToString();
        }

        #region Methods

        private void CalculateTotalPrice()
        {
            var valor = Produto.Valor;
            var quantidade = Quantidade;

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

        private async void AddToBasketButton_Clicked(object sender, EventArgs e)
        {
            var descricao = "Sem adicionais";
            if (AdicionaisSelecionados.Count > 0)
            {
                var adicionais = string.Join(", ", AdicionaisSelecionados.Select(a => a.Descricao));
                descricao = adicionais.Length > 60 ? adicionais.Substring(0, 57) + "..." : adicionais;
            }
            
            var valorTotal = Convert.ToDouble(lblTotalPrice.Text.Replace("R$ ", ""));
            var imageDownArrow = Quantidade == 1 ? "remove.png" : "arrow_down.png";

            OrderItem orderItem = new OrderItem(Guid.NewGuid(),
                                                Produto.ID,
                                                Produto.Nome,
                                                descricao,
                                                Produto.Valor,
                                                Quantidade,
                                                valorTotal,
                                                AdicionaisSelecionados.ToList(),
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