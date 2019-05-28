﻿using GarcOn.Models;
using GarcOn.Pages;
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
        public static bool IsAdmin { get; set; }
        public static List<Categoria> Categorias { get; set; }

        public static Dictionary<Produto, int> ItensPedido { get; set; }
        public static Dictionary<Produto, int> ItensPedidosFinalizados { get; set; }

        public App()
        {
            InitializeComponent();

            //Set default culture to get correct currency format
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");

            Task.Run(async () => 
            {
                var jsonData = await SecureStorage.GetAsync("categorias_e_produtos");
                if (string.IsNullOrEmpty(jsonData))
                {
                    Categorias = new List<Categoria>();
                }
                else
                {
                    Categorias = JsonConvert.DeserializeObject<List<Categoria>>(jsonData);
                }

            }).Wait();

            ItensPedido = new Dictionary<Produto, int>();
            ItensPedidosFinalizados = new Dictionary<Produto, int>();

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