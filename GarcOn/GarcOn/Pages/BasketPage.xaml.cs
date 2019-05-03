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
        const int MinValue = 1;
        const int MaxValue = 50;

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
                                                    produto.Valor,
                                                    quantidade,
                                                    produto.Valor * quantidade
                                                    //, Todo: Adicionar também a foto
                                                    );

                valorTotal += produto.Valor * quantidade;

                orderItems.Add(orderItem);
            }
            
            orders.ItemsSource = orderItems;

            lblTotalPrice.Text = string.Format("{0:C}", valorTotal);
        }

        private void ButtonUp_OnClicked(object sender, EventArgs e)
        {
            var b = (Button)sender;
            var ob = b.CommandParameter as OrderItem;

            if (ob != null)
            {
                if (MaxValue > ob.Quantity)
                    ob.Quantity += 1;

                ob.TotalPrice = ob.UnitPrice * ob.Quantity;
            }
        }

        private void ButtonDown_OnClicked(object sender, EventArgs e)
        {
            var b = (Button)sender;
            var ob = b.CommandParameter as OrderItem;

            if (ob != null)
            {
                if (MinValue < ob.Quantity)
                    ob.Quantity -= 1;

                ob.TotalPrice = ob.UnitPrice * ob.Quantity;
            }
        }

        public class OrderItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public double UnitPrice { get; set; }
            public int Quantity { get; set; }
            public double TotalPrice { get; set; }

            public OrderItem(string name, string description, double unitPrice, int quantity, double totalPrice)
            {
                this.Name = name;
                this.Description = description;
                this.UnitPrice = unitPrice;
                this.Quantity = quantity;
                this.TotalPrice = totalPrice;
            }

            public override string ToString()
            {
                return this.Name;
            }
        }
    }
}