using Android.Content;
using Android.OS;
using GarcOn.Controls;
using GarcOn.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FlatButton), typeof(FlatButtonRenderer))]
namespace GarcOn.Droid.Renderers
{
    public class FlatButtonRenderer : ButtonRenderer
    {
        public FlatButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                Control.StateListAnimator = null;
        }
    }
}