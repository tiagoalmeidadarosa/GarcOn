using GarcOn.NativeDependency;
using Xamarin.Forms;

[assembly: Dependency(typeof(GarcOn.Droid.NativeDependency.ProgressDialog))]
namespace GarcOn.Droid.NativeDependency
{
    public class ProgressDialog : IProgressDialog
    {
        Android.App.ProgressDialog progress;

        public void LoadingShow()
        {
            progress = new Android.App.ProgressDialog(Forms.Context);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage("Carregando, por favor aguarde...");
            progress.SetCancelable(false);
            progress.Show();
        }

        public void LoadingHide()
        {
            progress.Hide();
        }
    }
}