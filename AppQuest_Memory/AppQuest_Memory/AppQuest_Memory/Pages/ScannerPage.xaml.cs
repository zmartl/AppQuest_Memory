using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace AppQuest_Memory.Pages
{
    public partial class ScannerPage : ContentPage
    {
        public ScannerPage()
        {
            InitializeComponent();
        }

        private async void ScanButton_OnClicked(object sender, EventArgs e)
        {
            var scanPage = new ZXingScannerPage();

            scanPage.OnScanResult += (result) => {
                // Stop scanning
                scanPage.IsScanning = false;

                // Pop the page and show the result
                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopModalAsync();
                    DisplayAlert("Scanned Barcode", result.Text, "OK");
                });
            };

            // Navigate to our scanner page
            await Navigation.PushModalAsync(scanPage);
        }
    }
}
