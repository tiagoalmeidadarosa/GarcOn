using GarcOn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GarcOn.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FoodDetailPage : ContentPage
	{
        const int MinValue = 1;
        const int MaxValue = 50;

		public FoodDetailPage(Produto produto)
		{
			InitializeComponent();

            lblTitle.Text = produto.Nome;
            lblPrice.Text = "R$ " + produto.Valor.ToString("N2");
            lblDescription.Text = produto.Descricao;

            lblQtd.Text = MinValue.ToString();

            CalculateTotalPrice(produto.Valor, MinValue);
        }

        private async void PlusButton_OnClicked(object sender, EventArgs e)
        {
            var quantidade = Convert.ToInt16(lblQtd.Text);
            quantidade++;

            if (quantidade <= MaxValue)
            {
                try
                {
                    await lblQtd.ScaleTo(1.3, 100);
                    lblQtd.Text = quantidade.ToString();
                    await lblQtd.ScaleTo(1, 100);

                    var valor = Convert.ToDouble(lblPrice.Text.Replace("R$ ", ""));
                    CalculateTotalPrice(valor, quantidade);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private async void MinusButton_OnClicked(object sender, EventArgs e)
        {
            var quantidade = Convert.ToInt16(lblQtd.Text);
            quantidade--;

            if (quantidade >= MinValue)
            {
                try
                {
                    await lblQtd.ScaleTo(1.3, 100);
                    lblQtd.Text = quantidade.ToString();
                    await lblQtd.ScaleTo(1, 100);

                    var valor = Convert.ToDouble(lblPrice.Text.Replace("R$ ", ""));
                    CalculateTotalPrice(valor, quantidade);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void CalculateTotalPrice(double value, int quantity)
        {
            lblTotalPrice.Text = "Preço total: R$ " + (value * quantity).ToString("N2");
        }
    }
}