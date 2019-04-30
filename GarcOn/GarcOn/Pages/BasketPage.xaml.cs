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
    public partial class BasketPage : ContentPage
    {
        public BasketPage()
        {
            InitializeComponent();

            List<OrderItem> orderItems = new List<OrderItem>();
            double valorTotal = 0;

            foreach (var itemPedido in App.ItensPedido)
            {
                Produto produto = itemPedido.Key;
                int quantidade = itemPedido.Value;

                OrderItem orderItem = new OrderItem(produto.Nome, 
                                                    produto.Descricao, 
                                                    string.Format("{0:C}", produto.Valor * quantidade), 
                                                    quantidade.ToString()
                                                    //, Todo: Adicionar também a foto
                                                    );

                valorTotal += produto.Valor * quantidade;

                orderItems.Add(orderItem);
            }
            
            orders.ItemsSource = orderItems;

            lblTotalPrice.Text = string.Format("{0:C}", valorTotal);
        }

        public class OrderItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string TotalPrice { get; set; }
            public string Quantity { get; set; }

            public OrderItem(string name, string description, string totalPrice, string quantity)
            {
                this.Name = name;
                this.Description = description;
                this.TotalPrice = totalPrice;
                this.Quantity = quantity;
            }

            public override string ToString()
            {
                return this.Name;
            }
        }
    }
}