using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
	public partial class RequestAccountPopupPage : PopupPage
    {
		public RequestAccountPopupPage ()
		{
			InitializeComponent();

            for (int i = 0; i < 3; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = GridLength.Auto;
                gridItens.ColumnDefinitions.Add(columnDefinition);
            }

            var count = 0;
            foreach (var item in App.ItensPedido)
            {
                var produto = item.Key;
                var quantidade = item.Value;

                for (int i = 0; i < 3; i++)
                {
                    var label = new Label();
                    label.TextColor = Color.Black;
                    label.HorizontalTextAlignment = TextAlignment.Start;
                    label.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));

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
            labelTotal.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            labelTotal.Text = "Total: ";
            gridItens.Children.Add(labelTotal, 1, count + 1);

            var labelValorTotal = new Label();
            labelValorTotal.TextColor = Color.Black;
            labelValorTotal.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            labelValorTotal.Text = string.Format("{0:C}", App.ItensPedido.Sum(i => i.Value * i.Key.Valor));
            gridItens.Children.Add(labelValorTotal, 2, count + 1);
        }

        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {

        }

        private void ConfirmButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}