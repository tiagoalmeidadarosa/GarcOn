using GarcOn.Models;
using GarcOn.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GarcOn.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPageMaster : ContentPage
    {
        public ListView ListView;

        public MenuPageMaster()
        {
            InitializeComponent();

            BindingContext = new MenuPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MenuPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MenuPageMenuItem> MenuItems { get; set; }
            
            public MenuPageMasterViewModel()
            {
                var menuPageMenuItems = new List<MenuPageMenuItem>();

                menuPageMenuItems.Add(new MenuPageMenuItem { Image = "header_salgados.png", TargetType = null, LabelVisible = "False" });
                menuPageMenuItems.AddRange(GetCategories(CategoryType.Salgado));

                menuPageMenuItems.Add(new MenuPageMenuItem { Image = "header_bebidas.png", TargetType = null, LabelVisible = "False" });
                menuPageMenuItems.AddRange(GetCategories(CategoryType.Bebida));

                menuPageMenuItems.Add(new MenuPageMenuItem { Image = "header_doces.png", TargetType = null, LabelVisible = "False" });
                menuPageMenuItems.AddRange(GetCategories(CategoryType.Doce));

                MenuItems = new ObservableCollection<MenuPageMenuItem>(menuPageMenuItems);
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion

            private List<MenuPageMenuItem> GetCategories(CategoryType tipo)
            {
                var menuPageMenuItems = new List<MenuPageMenuItem>();

                foreach (var categoria in App.Categorias.Where(c => c.Tipo == (int)tipo))
                {
                    menuPageMenuItems
                        .Add(
                            new MenuPageMenuItem
                            {
                                Id = categoria.ID,
                                Title = categoria.Descricao.ToUpper(),
                                TargetType = typeof(FoodsPage),
                                LabelVisible = "True"
                            }
                        );
                }

                return menuPageMenuItems;
            }
        }

        private async void FinalizeAccountButton_Clicked(object sender, EventArgs e)
        {
            var completeAccount = await DisplayAlert("Fechar conta", "Deseja realmente solicitar a conta?", "Sim", "Não");

            if (completeAccount)
            {
                await PopupNavigation.Instance.PushAsync(new RequestAccountPopupPage());

                //var ip = await SecureStorage.GetAsync("ip_servidor");
                //var numeroMesa = Convert.ToInt32(await SecureStorage.GetAsync("numero_mesa"));
                //var valorTotal = App.ItensPedido.Sum(i => i.Key.Valor * i.Value);
                ///*var valorTotal = Convert.ToDouble(lblTotalPrice.Text.Replace("R$ ", ""));

                //Dictionary<long, int> itensPedido = new Dictionary<long, int>();
                //foreach (var itemPedido in App.ItensPedido)
                //{
                //    itensPedido.Add(itemPedido.Key.ID, itemPedido.Value);
                //}*/

                //APIService apiService = new APIService(ip);
                //var errorMessage = apiService.AddAccountRequest(numeroMesa, valorTotal);

                //if (string.IsNullOrEmpty(errorMessage))
                //{
                //    await DisplayAlert("Confirmação de fechamento de conta", "Sua solicitação foi cadastrada com sucesso, aguarde um momento que alguém irá atendê-lo. :)", "Fechar");

                //    //App.ItensPedido = new Dictionary<Produto, int>();
                //    App.Current.MainPage = new MenuPage();
                //}
                //else
                //{
                //    await DisplayAlert("Erro no fechamento da conta", "Não foi possível cadastrar a solicitação, talvez o servidor não esteja respondendo, tente novamente em alguns segundos. Erro: " + errorMessage, "Fechar");
                //}
            }
        }
    }
}