using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondLauncherV2.Utilities
{
    class ToastUtils
    {
        public static void ShowToast(string title, string message)
        {
            new ToastContentBuilder()
                .AddText(title)
                .AddText(message)
                .Show();
        }
    }
}
