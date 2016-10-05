using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AppQuest_Memory.Annotations;
using Newtonsoft.Json;

namespace AppQuest_Memory.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MemoryGroup : ObservableCollection<MemoryItem>
    {
        private string _name;

        public MemoryGroup()
        {
            
        }

        public MemoryGroup(string name)
            : base(new [] { new MemoryItem(), new MemoryItem() })
        {
            Name = name;
        }

        [JsonProperty("ProxyItems")]
        public IList<MemoryItem> ProxyItems
        {
            get { return Items; }
            set
            {
                Items.Clear();
                foreach (var item in value)
                {
                    Items.Add(item);
                }
            }
        }

        [JsonProperty("Name")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (Equals(_name, value))
                    return;
                _name = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Name)));
            }
        }
    }

    public class MemoryItem : INotifyPropertyChanged
    {
        private string _title = "(Noch kein Scan)";

        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}