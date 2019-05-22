using Android.Content;
using Android.Views;
using GarcOn.Controls;
using GarcOn.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(CustomViewCell), typeof(CustomViewCellRenderer))]

namespace GarcOn.Droid.Renderers
{
    public class CustomViewCellRenderer : ViewCellRenderer
    {
        protected override View GetCellCore(Cell item, View convertView, ViewGroup parent, Context context)
        {
            var cell = base.GetCellCore(item, convertView, parent, context);
            cell.SetBackgroundResource(Resource.Drawable.ViewCellBackground);
            return cell;
        }
    }
}