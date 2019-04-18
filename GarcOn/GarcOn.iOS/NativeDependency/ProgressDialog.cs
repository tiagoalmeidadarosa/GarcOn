using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using GarcOn.iOS.NativeDependency;
using GarcOn.NativeDependency;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(ProgressDialog))]
namespace GarcOn.iOS.NativeDependency
{
    public class ProgressDialog : IProgressDialog
    {
        //public static MTMBProgressHUD progress;
        //public static UIWindow window;

        public void LoadingShow()
        {
            /*window = UIApplication.SharedApplication.KeyWindow;
            window.UserInteractionEnabled = false;
            window.Alpha = 0.75f;

            var view = window.ViewForBaselineLayout;

            progress = new MTMBProgressHUD(view)
            {
                LabelText = "Carregando, aguarde...",
                RemoveFromSuperViewOnHide = true,
            };
            progress.Alpha = 0.75f;

            view.AddSubview(progress);
            progress.Show(animated: true);*/
        }

        public void LoadingHide()
        {
            /*if (progress != null)
            {
                if (window != null)
                {
                    window.UserInteractionEnabled = true;
                    window.Alpha = 1f;
                }

                progress.Hide(animated: true, delay: 1);
            }*/
        }
    }
}