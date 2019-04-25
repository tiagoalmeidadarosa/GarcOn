using GarcOn.Models;
using System;
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
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            long idCategoria = string.IsNullOrEmpty(this.ClassId) ? 0 : Convert.ToInt64(ClassId);

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