using GarcOn.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GarcOn
{
    public partial class App : Application
    {
        public static bool _isAdmin { get; set; }

        public App()
        {
            InitializeComponent();

            MainPage = new MenuPage();
        }

        protected override void OnStart()
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
