using GarcOn.Models;
using System.Collections.Generic;
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
                    FoodsList.ItemsSource = categoria.Produtos.ToList();
                }
            }
            else
            {
                var produtos = new List<Produto>();
                foreach (var categoria in App.Categorias)
                {
                    produtos.AddRange(categoria.Produtos.ToList());
                }

                FoodsList.ItemsSource = produtos;
            }
        }
    }
}