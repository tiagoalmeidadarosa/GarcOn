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
	public partial class FoodsPage : ContentPage
	{
		public FoodsPage ()
		{
			InitializeComponent ();

            FoodsList.ItemsSource = new[]
                {
                    new MenuPageMenuItem { Id = 0, Title = "Burguer", TargetType = typeof(BasketPage) },
                    new MenuPageMenuItem { Id = 1, Title = "Baião de 2" }
                };
        }
	}
}