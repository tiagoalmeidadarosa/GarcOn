using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GarcOn.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FoodsPage : ContentPage
    {
        public FoodsPage()
        {
            //Todo: Fazer o idCategoria virar um parâmetro, algo bem complexo
            long idCategoria = 0;

            InitializeComponent();

            if (idCategoria > 0)
            {
                var categoria = App.Categorias.FirstOrDefault(c => c.ID == idCategoria);
                if (categoria != null)
                {
                    FoodsList.ItemsSource = categoria.Produtos;
                }
            }
            else
            {
                FoodsList.ItemsSource = App.Categorias.Select(c => c.Produtos);
            }

            /*FoodsList.ItemsSource = new[]
                {
                    new MenuPageMenuItem { Id = 0, Title = "Burguer", TargetType = typeof(BasketPage) },
                    new MenuPageMenuItem { Id = 1, Title = "Baião de 2" }
                };*/
        }
    }
}