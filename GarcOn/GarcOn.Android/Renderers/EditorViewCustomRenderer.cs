using System;

using Android.Content;
using Android.Views;
using GarcOn.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Editor), typeof(EditorViewCustomRenderer))]
namespace GarcOn.Droid.Renderers
{
    public class EditorViewCustomRenderer : EditorRenderer
    {
        public EditorViewCustomRenderer(Context context) : base(context)
        {
            //Não sei a real necessidade disso, mas tive que implementar este construtor pois ocorria mensagem de método obsoleto
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null && Convert.ToInt32(Android.OS.Build.VERSION.Sdk) > 19) //Rounded corners not supported version Android 4.4
            {
                Control.Background = Android.App.Application.Context.GetDrawable(Resource.Drawable.rounded_corners);
                Control.Gravity = GravityFlags.Top;
                Control.SetPadding(10, 0, 0, 0);
            }
        }
    }
}