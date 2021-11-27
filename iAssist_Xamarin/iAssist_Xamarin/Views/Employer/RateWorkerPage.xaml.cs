using iAssist_Xamarin.Models;
using LaavorRatingConception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iAssist_Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RateWorkerPage : ContentPage
    {
        public RateWorkerPage()
        {
            InitializeComponent();
        }

        private void RatingConception_Voted(object sender, EventArgs e)
        {
            RatingConception rating = (RatingConception)sender;
            TempRating.Rate = rating.Value;
        }
    }
}