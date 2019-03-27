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
    public partial class BasketPage : ContentPage
    {
        public BasketPage()
        {
            InitializeComponent();

            List<OrderItem> items = new List<OrderItem>();
            items.Add(new OrderItem("Hamburguer", "A hamburguer, beefburguer or burger is a sandwich consisting of one or more coocked patties of ground...", "100"));

            orders.ItemsSource = items;
        }

        public class OrderItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string TotalPrice { get; set; }            

            public OrderItem(string name, string description = "", string totalPrice = "")
            {
                this.Name = name;
                this.Description = description;
                this.TotalPrice = totalPrice;
            }

            public override string ToString()
            {
                return this.Name;
            }
        }
    }
}