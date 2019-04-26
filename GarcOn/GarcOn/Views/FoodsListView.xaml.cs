using GarcOn.Models;
using GarcOn.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GarcOn.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FoodsListView : ListView
    {
		public FoodsListView () : base(ListViewCachingStrategy.RecycleElement)
		{
			InitializeComponent();
		}

        private async void FoodsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }

            var item = (Produto)e.SelectedItem;

            if (item != null)
            {
                var foodDetailPage = new FoodDetailPage(item);
                foodDetailPage.Title = item.Nome;

                await Navigation.PushAsync(foodDetailPage);

                FoodsList.SelectedItem = null;
            }
        }
    }
}