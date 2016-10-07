using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using AppQuest_Memory.Model;
using AppQuest_Memory.Services;
using Newtonsoft.Json;
using PCLStorage;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace AppQuest_Memory.Pages
{
    public partial class HomePage : ContentPage
    {
        private const string FOLDER = "AppQuest_Memory";
        private const string FILE = "Data.json";
        private ObservableCollection<MemoryGroup> _groupedItems;
        private IFile _localFile;
        private IFolder _localFolder;
        private bool _noEntries = true;
        private IFolder _rootFolder;

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
                NoEntries = _groupedItems.Count == 0;
                OnPropertyChanged();
            }
        }

        public bool NoEntries
        {
            get { return _noEntries; }
            set
            {
                if (_noEntries == value) return;
                _noEntries = value;
                OnPropertyChanged();
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var rootFolder = FileSystem.Current.LocalStorage;

            _localFolder = await rootFolder.CreateFolderAsync(FOLDER, CreationCollisionOption.OpenIfExists);
            _localFile = await _localFolder.CreateFileAsync(FILE, CreationCollisionOption.OpenIfExists);
            var result = "";
            result = await _localFile.ReadAllTextAsync();

            if (result.Length > 0)
                FillListView(result);
        }

        public void FillListView(string file)
        {
            var json = JsonConvert.DeserializeObject<IEnumerable<MemoryGroup>>(file);
            GroupedItems =
                new ObservableCollection<MemoryGroup>(json);
        }

        private async Task SaveFile()
        {
            var json = JsonConvert.SerializeObject(GroupedItems);
            await _localFile.WriteAllTextAsync(json);
        }

        public async void GroupedItemsOnCollectionChanged(object sender,
            NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            NoEntries = GroupedItems.Count == 0;
            await SaveFile();
        }

        private void MenuItem_OnClicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.Prompt(new PromptConfig
            {
                Title = "Kategorie erstellen",
                InputType = InputType.Default,
                OkText = "Erstellen",
                CancelText = "Abbrechen",
                OnAction = result =>
                {
                    if (!result.Ok) return;

                    GroupedItems.Add(new MemoryGroup(result.Text));
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

                scanPage.OnScanResult += async (result) =>
                {
                    // Stop scanning
                    scanPage.IsScanning = false;

                    if (!string.IsNullOrEmpty(result.Text))
                    {
                        memoryItem.Title = result.Text;
                        await SaveFile();
                    }
                    else
                        await DisplayAlert("Warnung", "QR-Code konnte nicht gescannt werden.", "OK");

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

        private void BtnSend_OnClicked(object sender, EventArgs e)
        {
            var logBuch = DependencyService.Get<ILogBuchService>();

            var result = GroupedItems.Select(t => t.Select(x => x.Title));
            var json = JsonConvert.SerializeObject(result);
            logBuch.OpenLogBuch("Memory", json);
        }

        private async void MenuItem_OnClickeduItemClear_OnClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Löschen", "Willst du wirklich alles löschen?", "Ja", "Nein");

            if (result)
            {
                GroupedItems.Clear();
            }
        }

		public async void OnDeleteItem(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
			var item = (MemoryItem)mi.CommandParameter;
			item.Title = "(Noch kein Scan TEst)";
			await SaveFile();
        }

		//public void OnDeleteGroup(object sender, EventArgs e)
		//{
			//var mi = ((MenuItem)sender);
		//}
    }
    
}