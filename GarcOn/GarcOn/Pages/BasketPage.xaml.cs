using GarcOn.ViewModels;
using GarcOn.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GarcOn.Models;
using Newtonsoft.Json;

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

            foreach (var orderItem in App.ItensPedido)
            {
                orderItems.Add(orderItem);

                valorTotal += orderItem.TotalPrice;
            }
            
            Orders.ItemsSource = orderItems;

            lblTotalPrice.Text = string.Format("{0:C}", valorTotal);
        }

        protected override void OnAppearing()
        {
            if(App.ItensPedido.Count > 0)
            {
                btnFinalizeOrder.IsVisible = true;
            }
            else
            {
                btnFinalizeOrder.IsVisible = false;
            }
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

                foreach (var selectedAditional in ob.SelectedAditionals)
                {
                    ob.TotalPrice += selectedAditional.Valor * ob.Quantity;
                }
            }

            //Recoloca os itens com mudanças na lista
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in (List<OrderItem>)Orders.ItemsSource)
            {
                if (item.UniqueId == ob.UniqueId)
                {
                    orderItems.Add(ob);
                }
                else
                {
                    orderItems.Add(new OrderItem(item.UniqueId, 
                                                 item.Id, 
                                                 item.Name, 
                                                 item.Description, 
                                                 item.UnitPrice, 
                                                 item.Quantity, 
                                                 item.TotalPrice, 
                                                 item.SelectedAditionals));
                }
            }

            Orders.ItemsSource = orderItems;

            //Altera preço total na tela
            lblTotalPrice.Text = string.Format("{0:C}", orderItems.Sum(o => o.TotalPrice));

            //Altera item na variável global
            var itemPedido = App.ItensPedido.FirstOrDefault(i => i.UniqueId == ob.UniqueId);
            if (itemPedido != null)
            {
                App.ItensPedido.Remove(itemPedido);
            }

            App.ItensPedido.Add(ob);
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
                    remove = await DisplayAlert("REMOVER ITEM", "Deseja realmente remover o produto?", "SIM", "NÃO");
                }

                ob.TotalPrice = ob.UnitPrice * ob.Quantity;

                foreach (var selectedAditional in ob.SelectedAditionals)
                {
                    ob.TotalPrice += selectedAditional.Valor * ob.Quantity;
                }
            }

            //Recoloca os itens com mudanças na lista
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in (List<OrderItem>)Orders.ItemsSource)
            {
                if (item.UniqueId == ob.UniqueId)
                {
                    if (!remove)
                        orderItems.Add(ob);
                }
                else
                {
                    orderItems.Add(new OrderItem(item.UniqueId, 
                                                 item.Id, 
                                                 item.Name, 
                                                 item.Description, 
                                                 item.UnitPrice, 
                                                 item.Quantity, 
                                                 item.TotalPrice, 
                                                 item.SelectedAditionals));
                }
            }

            Orders.ItemsSource = orderItems;

            //Altera preço total na tela
            lblTotalPrice.Text = string.Format("{0:C}", orderItems.Sum(o => o.TotalPrice));

            //Altera item na variável global
            var itemPedido = App.ItensPedido.FirstOrDefault(i => i.UniqueId == ob.UniqueId);
            if (itemPedido != null)
            {
                App.ItensPedido.Remove(itemPedido);
            }

            if (!remove)
                App.ItensPedido.Add(ob);

            //Refaz OnAppearing da página
            OnAppearing();
        }

        private async void CompleteOrder_OnClicked(object sender, EventArgs e)
        {
            var completeOrder = await DisplayAlert("FINALIZAR PEDIDO", "Deseja realmente enviar o seu pedido para a cozinha?", "SIM", "NÃO");

            if (completeOrder)
            {
                var ip = await SecureStorage.GetAsync("ip_servidor");
                var numeroMesa = Convert.ToInt32(await SecureStorage.GetAsync("numero_mesa"));
                var valorTotal = Convert.ToDouble(lblTotalPrice.Text.Replace("R$ ", ""));

                List<ItemPedido> itensPedido = new List<ItemPedido>();
                foreach (var orderItem in App.ItensPedido)
                {
                    var itemPedido = new ItemPedido();
                    itemPedido.ID_Produto = orderItem.Id;
                    itemPedido.Quantidade = orderItem.Quantity;
                    itemPedido.AdicionaisSelecionados = orderItem.SelectedAditionals;

                    itensPedido.Add(itemPedido);
                }

                APIService apiService = new APIService(ip);
                var errorMessage = apiService.AddOrder(numeroMesa, valorTotal, JsonConvert.SerializeObject(itensPedido));

                if (string.IsNullOrEmpty(errorMessage))
                {
                    App.ItensPedidosFinalizados.AddRange(App.ItensPedido);

                    await DisplayAlert("CONFIRMAÇÃO DO PEDIDO", "Seu pedido foi cadastrado com sucesso.", "FECHAR");

                    App.ItensPedido = new List<OrderItem>();
                    App.Current.MainPage = new MenuPage();
                }
                else
                {
                    await DisplayAlert("ERRO NA CONFIRMAÇÃO DO PEDIDO", "Não foi possível cadastrar o pedido, talvez o servidor não esteja respondendo, tente novamente em alguns instantes. Erro: " + errorMessage, "FECHAR");
                }
            }
        }

        private void Orders_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Orders.SelectedItem = null;
        }
    }
}