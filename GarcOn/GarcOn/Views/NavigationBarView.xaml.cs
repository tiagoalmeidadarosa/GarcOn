using GarcOn.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GarcOn.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NavigationBarView : ContentView
	{
		public NavigationBarView()
		{
			InitializeComponent();
		}

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Tapcart_OnTapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BasketPage());
        }


        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(
                "Title",
                typeof(string),
                typeof(NavigationBarView),
                "Título da Página",
                propertyChanged: OnTitlePropertyChanged
            );

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        static void OnTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisView = bindable as NavigationBarView;
            var title = newValue.ToString();

            thisView.lblTitlePage.Text = title;
        }


        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create(
                "Color",
                typeof(Color),
                typeof(NavigationBarView),
                Color.FromHex("#361A11"),
                propertyChanged: OnColorPropertyChanged
            );

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        static void OnColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisView = bindable as NavigationBarView;
            var color = (Color)newValue;

            thisView.gridNavigationBar.BackgroundColor = color;
        }


        public static readonly BindableProperty BasketValueProperty =
            BindableProperty.Create(
                "BasketValue",
                typeof(string),
                typeof(NavigationBarView),
                "0",
                propertyChanged: OnBasketValuePropertyChanged
            );

        public string BasketValue
        {
            get { return (string)GetValue(BasketValueProperty); }
            set { SetValue(BasketValueProperty, value); }
        }

        static void OnBasketValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisView = bindable as NavigationBarView;
            var basketValue = newValue.ToString();

            thisView.labelText.Text = basketValue;
        }


        public static readonly BindableProperty BackProperty =
            BindableProperty.Create(
                "Back",
                typeof(bool),
                typeof(NavigationBarView),
                true,
                propertyChanged: OnBackPropertyChanged
            );

        public bool Back
        {
            get { return (bool)GetValue(BackProperty); }
            set { SetValue(BackProperty, value); }
        }

        static void OnBackPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisView = bindable as NavigationBarView;
            var back = (bool)newValue;

            thisView.contentBack.IsVisible = back;
        }


        public static readonly BindableProperty BasketProperty =
            BindableProperty.Create(
                "Basket",
                typeof(bool),
                typeof(NavigationBarView),
                true,
                propertyChanged: OnBasketPropertyChanged
            );

        public bool Basket
        {
            get { return (bool)GetValue(BasketProperty); }
            set { SetValue(BasketProperty, value); }
        }

        static void OnBasketPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisView = bindable as NavigationBarView;
            var basket = (bool)newValue;

            thisView.contentBasket.IsVisible = basket;
        }
    }
}