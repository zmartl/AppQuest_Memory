using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AppQuest_Memory.Pages
{
    public partial class NavPage : ContentPage
    {

        public ListView ListView => NavListView;

        public NavPage()
        {
            InitializeComponent();

            NavListView.ItemsSource = new List<StartScreen>()
            {
                new StartScreen
                {
                    Title = "Kamera",
                    Image = "qrcode_image.png"
                }
            };

        }
    }
}
