using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppQuest_Memory.Model;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace AppQuest_Memory.Pages
{
    public partial class ScannerPage : ContentPage
    {
        private readonly MemoryItem _memoryItem;

        public ScannerPage(MemoryItem memoryItem)
        {
            _memoryItem = memoryItem;
            InitializeComponent();
        }

        private async void ScanButton_OnClicked(object sender, EventArgs e)
        {
            var scanPage = new ZXingScannerPage();

            scanPage.OnScanResult += (result) => {
                // Stop scanning
                scanPage.IsScanning = false;
                _memoryItem.Title = result.Text;
                // Pop the page and show the result
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopModalAsync();
                });
            };

            // Navigate to our scanner page
            await Navigation.PushModalAsync(scanPage);
        }
    }
}
