using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
                MenuItems = new ObservableCollection<MenuPageMenuItem>(new[]
                {
                    new MenuPageMenuItem { Image = "header_salgados.png", TargetType = null, LabelVisible = "False" },
                    new MenuPageMenuItem { Id = 0, Title = "FOODS PAGE", TargetType = typeof(FoodsPage), LabelVisible = "True" },
                    new MenuPageMenuItem { Image = "header_bebidas.png", TargetType = null, LabelVisible = "False" },
                    new MenuPageMenuItem { Id = 1, Title = "FOOD DETAIL PAGE", TargetType = typeof(FoodDetailPage), LabelVisible = "True" },
                    new MenuPageMenuItem { Image = "header_doces.png", TargetType = null, LabelVisible = "False" },
                    new MenuPageMenuItem { Id = 2, Title = "BASKET PAGE", TargetType = typeof(BasketPage), LabelVisible = "True" },
                    new MenuPageMenuItem { Image = "header_configuracoes.png", TargetType = null, LabelVisible = "False" },
                    new MenuPageMenuItem { Id = 3, Title = "CONFIGURATION PAGE", TargetType = typeof(ConfigurationPage), LabelVisible = "True" }
                });
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
        }
    }
}