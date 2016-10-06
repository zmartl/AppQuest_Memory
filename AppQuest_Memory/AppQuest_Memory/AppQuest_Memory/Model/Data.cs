using AppQuest_Memory.Model;
using System.Collections.ObjectModel;

namespace AppQuest_Memory.Pages
{
    internal class Data
    {
        public static ObservableCollection<QRCode> List = new ObservableCollection<QRCode>
        {
            new QRCode {Name = "Test"},
            new QRCode {Name = "blub" }
        };

    }
}