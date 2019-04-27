using GarcOn.Models;
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
                                Title = categoria.Descricao,
                                TargetType = typeof(FoodsPage),
                                LabelVisible = "True"
                            }
                        );
                }

                return menuPageMenuItems;
            }
        }
    }
}