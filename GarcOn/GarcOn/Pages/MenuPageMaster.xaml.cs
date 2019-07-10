using GarcOn.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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

        protected override void OnAppearing()
        {
            if(App.ItensPedidosFinalizados.Count > 0)
            {
                MenuItemsListView.Margin = new Thickness(0, 0, 0, 60);
                btnFinalizeAccount.IsVisible = true;
            }
            else
            {
                MenuItemsListView.Margin = new Thickness(0, 0, 0, 0);
                btnFinalizeAccount.IsVisible = false;
            }
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
            await PopupNavigation.Instance.PushAsync(new RequestAccountPopupPage());
        }

        Stopwatch stopWatch = new Stopwatch();
        public int countClick = 0;

        private async void CoverImage_TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            countClick++;

            if (countClick == 1)
            {
                stopWatch.Start();
                return;
            }

            if(countClick == 5 && stopWatch.Elapsed <= TimeSpan.FromSeconds(5))
            {
                countClick = 0;
                stopWatch.Reset();

                await PopupNavigation.Instance.PushAsync(new RequestAccountPopupPage(true));
            }
            else if (stopWatch.Elapsed > TimeSpan.FromSeconds(5))
            {
                countClick = 0;
                stopWatch.Reset();
            }
        }
    }
}