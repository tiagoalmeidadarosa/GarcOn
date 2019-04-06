using Xamarin.Forms;

namespace GarcOn.Controls
{
    public class CardView : Frame
    {
        public CardView()
        {
            Padding = 0;
            if (Device.RuntimePlatform == "iOS")
            {
                Margin = 0;
                HasShadow = false;
                BorderColor = Color.Transparent;
                BackgroundColor = Color.Transparent;
            }
        }
    }
}
