using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace iAssist_Xamarin.Helpers
{
    public interface IEnvironment
    {
        void SetStatusBarColor(Color color, bool darkStatusBarTint);
    }
}
