using GarcOn.Models;
using GarcOn.Pages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GarcOn
{
    public partial class App : Application
    {
        public static bool IsAdmin { get; set; }
        public static List<Categoria> Categorias { get; set; }

        public App()
        {
            InitializeComponent();

            Task.Run(async () => {

                var jsonCategoriesAndProducts = await SecureStorage.GetAsync("categorias_e_produtos");
                if (string.IsNullOrEmpty(jsonCategoriesAndProducts))
                {
                    Categorias = new List<Categoria>();
                }
                else
                {
                    Categorias = JsonConvert.DeserializeObject<List<Categoria>>(jsonCategoriesAndProducts);
                }

            }).Wait();
#if DEBUG
            HotReloader.Current.Start(this);
#endif
            MainPage = new MenuPage();
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