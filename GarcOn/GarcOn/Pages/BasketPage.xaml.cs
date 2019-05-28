using GarcOn.Models;
using GarcOn.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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

                OrderItem orderItem = new OrderItem(produto,
                                                    produto.ID,
                                                    produto.Nome, 
                                                    produto.Descricao,
                                                    produto.Valor,
                                                    quantidade,
                                                    produto.Valor * quantidade
                                                    //, Todo: Adicionar também a foto
                                                    );

                valorTotal += produto.Valor * quantidade;

                orderItems.Add(orderItem);
            }
            
            Orders.ItemsSource = orderItems;

            lblTotalPrice.Text = string.Format("{0:C}", valorTotal);
        }

        private void ButtonUp_OnClicked(object sender, EventArgs e)
        {
            var b = (Button)sender;
            var ob = b.CommandParameter as OrderItem;

            if (ob == null)
            {
                return;
            }
            else
            {
                if (MaxValue > ob.Quantity)
                    ob.Quantity += 1;

                ob.TotalPrice = ob.UnitPrice * ob.Quantity;
            }

            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in (List<OrderItem>)Orders.ItemsSource)
            {
                if (item.Id == ob.Id)
                {
                    orderItems.Add(ob);
                }
                else
                {
                    orderItems.Add(new OrderItem(item.Produto, item.Id, item.Name, item.Description, item.UnitPrice, item.Quantity, item.TotalPrice));
                }
            }

            Orders.ItemsSource = orderItems;

            //Altera preço total na tela
            lblTotalPrice.Text = string.Format("{0:C}", orderItems.Sum(o => o.TotalPrice));

            //Altera quantidade na variável global
            if (App.ItensPedido.ContainsKey(ob.Produto))
            {
                App.ItensPedido.Remove(ob.Produto);
            }

            App.ItensPedido.Add(ob.Produto, ob.Quantity);
        }

        private async void ButtonDown_OnClicked(object sender, EventArgs e)
        {
            var remove = false;

            var b = (Button)sender;
            var ob = b.CommandParameter as OrderItem;

            if (ob == null)
            {
                return;
            }
            else
            {
                if (MinValue < ob.Quantity)
                {
                    ob.Quantity -= 1;
                }
                else
                {
                    remove = await DisplayAlert("Remover Item", "Deseja realmente remover o produto?", "Sim", "Não");
                }

                ob.TotalPrice = ob.UnitPrice * ob.Quantity;
            }

            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in (List<OrderItem>)Orders.ItemsSource)
            {
                if (item.Id == ob.Id)
                {
                    if(!remove)
                        orderItems.Add(ob);
                }
                else
                {
                    orderItems.Add(new OrderItem(item.Produto, item.Id, item.Name, item.Description, item.UnitPrice, item.Quantity, item.TotalPrice));
                }
            }

            Orders.ItemsSource = orderItems;

            //Altera preço total na tela
            lblTotalPrice.Text = string.Format("{0:C}", orderItems.Sum(o => o.TotalPrice));

            //Altera quantidade na variável global
            if (App.ItensPedido.ContainsKey(ob.Produto))
            {
                App.ItensPedido.Remove(ob.Produto);
            }

            if (!remove)
                App.ItensPedido.Add(ob.Produto, ob.Quantity);
        }

        private async void CompleteOrder_OnClicked(object sender, EventArgs e)
        {
            var completeOrder = await DisplayAlert("Finalizar Pedido", "Deseja realmente finalizar o seu pedido?", "Sim", "Não");

            if (completeOrder)
            {
                var ip = await SecureStorage.GetAsync("ip_servidor");
                var numeroMesa = Convert.ToInt32(await SecureStorage.GetAsync("numero_mesa"));
                var valorTotal = Convert.ToDouble(lblTotalPrice.Text.Replace("R$ ", ""));

                Dictionary<long, int> itensPedido = new Dictionary<long, int>();
                foreach (var itemPedido in App.ItensPedido)
                {
                    itensPedido.Add(itemPedido.Key.ID, itemPedido.Value);
                }

                APIService apiService = new APIService(ip);
                var errorMessage = apiService.AddOrder(numeroMesa, valorTotal, itensPedido);

                if (string.IsNullOrEmpty(errorMessage))
                {
                    foreach (var itemPedido in App.ItensPedido)
                    {
                        var produto = itemPedido.Key;
                        var quantidade = itemPedido.Value;

                        if (App.ItensPedidosFinalizados.ContainsKey(produto))
                        {
                            quantidade += App.ItensPedidosFinalizados[produto];
                            App.ItensPedidosFinalizados.Remove(produto);
                        }

                        App.ItensPedidosFinalizados.Add(produto, quantidade);
                    }

                    await DisplayAlert("Confirmação do Pedido", "Seu pedido foi cadastrado com sucesso.", "Fechar");

                    App.ItensPedido = new Dictionary<Produto, int>();
                    App.Current.MainPage = new MenuPage();
                }
                else
                {
                    await DisplayAlert("Erro na Confirmação do Pedido", "Não foi possível cadastrar o pedido, talvez o servidor não esteja respondendo, tente novamente em alguns instantes. Erro: " + errorMessage, "Fechar");
                }
            }
        }

        private void Orders_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Orders.SelectedItem = null;
        }

        public class OrderItem
        {
            public Produto Produto { get; set; }
            public long Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public double UnitPrice { get; set; }
            public int Quantity { get; set; }
            public double TotalPrice { get; set; }

            public OrderItem(Produto produto, long id, string name, string description, double unitPrice, int quantity, double totalPrice)
            {
                this.Produto = produto;
                this.Id = id;
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