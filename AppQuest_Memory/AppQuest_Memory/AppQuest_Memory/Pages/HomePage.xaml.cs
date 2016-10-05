using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Acr.UserDialogs;
using AppQuest_Memory.Model;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace AppQuest_Memory.Pages
{
    public partial class HomePage : ContentPage
    {
        private ObservableCollection<MemoryGroup> _groupedItems;
        private bool _noEntries = true;

        public HomePage()
        {
            InitializeComponent();
            GroupedItems = new ObservableCollection<MemoryGroup>();
            BindingContext = this;
        }

        public ObservableCollection<MemoryGroup> GroupedItems
        {
            get { return _groupedItems; }
            set
            {
                if (Equals(_groupedItems, value)) return;
                if (_groupedItems != null)
                    _groupedItems.CollectionChanged -= GroupedItemsOnCollectionChanged;
                _groupedItems = value;
                _groupedItems.CollectionChanged += GroupedItemsOnCollectionChanged;
                OnPropertyChanged();
            }
        }

        private void GroupedItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            NoEntries = GroupedItems.Count == 0;
        }

        public bool NoEntries
        {
            get { return _noEntries; }
            set
            {
                if(_noEntries == value) return;                
                _noEntries = value;
                OnPropertyChanged();
            }
        }

        private void MenuItem_OnClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.Prompt(new PromptConfig
            {
                Title = "Gib ein",
                InputType = InputType.Default,
                OkText = "Erstellen",
                CancelText = "Abbrechen",
                OnAction = result =>
                {
                    if (!result.Ok) return;
                    GroupedItems.Add(new MemoryGroup {Name = result.Text});
                }
            });
        }

        private void GroupListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is MemoryItem)
            {
                var memoryItem = e.SelectedItem as MemoryItem;

                // open ScanPage

                var scanPage = new ZXingScannerPage();

                scanPage.OnScanResult += result =>
                {
                    // Stop scanning
                    scanPage.IsScanning = false;

                    if (!string.IsNullOrEmpty(result.Text))
                        memoryItem.Title = result.Text;
                    else
                        DisplayAlert("Warnung", "QR-Code konnte nicht gescannt werden.", "OK");
                    // Pop the page and show the result
                    Device.BeginInvokeOnMainThread(() => { Navigation.PopModalAsync(); });
                };

                // Navigate to our scanner page
                Navigation.PushModalAsync(scanPage);
            }
            else if (e.SelectedItem is MemoryGroup)
            {
                var group = e.SelectedItem as MemoryGroup;
                UserDialogs.Instance.Prompt(new PromptConfig
                {
                    Title = "Gib ein",
                    InputType = InputType.Default,
                    Text = group.Name,
                    OkText = "Erstellen",
                    CancelText = "Abbrechen",
                    OnAction = result =>
                    {
                        if (!result.Ok) return;
                        group.Name = result.Text;
                    }
                });
            }
            ((ListView) sender).SelectedItem = null;
        }
    }
}