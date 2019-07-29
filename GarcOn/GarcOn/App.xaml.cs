using GarcOn.Models;
using GarcOn.Pages;
using GarcOn.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GarcOn
{
    public partial class App : Application
    {
        public static string User { get; set; }
        public static string Password { get; set; }

        public static List<Categoria> Categorias { get; set; }

        public static List<OrderItem> ItensPedido { get; set; }
        public static List<OrderItem> ItensPedidosFinalizados { get; set; }
        public static List<OrderItem> ItensPedidosFinalizadosUltimaConta { get; set; }

        public App()
        {
            InitializeComponent();

            //Set default culture to get correct currency format
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");

            Categorias = new List<Categoria>();
            ItensPedido = new List<OrderItem>();
            ItensPedidosFinalizados = new List<OrderItem>();
            ItensPedidosFinalizadosUltimaConta = new List<OrderItem>();

#if DEBUG
            HotReloader.Current.Start(this);
#endif
            MainPage = new ConfigurationPage();
        }

        protected override async void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}