using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AppQuest_Memory.Annotations;

namespace AppQuest_Memory.Model
{
    public class MemoryGroup : List<MemoryItem>
    {
        public MemoryGroup()
            : base(new []{ new MemoryItem(), new MemoryItem() })
        {
            
        }

        public string Name { get; set; }
    }

    public class MemoryItem : INotifyPropertyChanged
    {
        private string _title = "(Noch kein Scan)";
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        public string Image { get; set; }
    }
}