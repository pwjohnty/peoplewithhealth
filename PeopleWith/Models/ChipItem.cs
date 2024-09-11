using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
namespace PeopleWith
{
    public class ChipItem
    {
        public string Text { get; set; }
        public bool IsSelected { get; set; }
    }
    public class ChipItems : INotifyPropertyChanged
    {
        private ObservableCollection<ChipItem> chipListView;
        public ObservableCollection<ChipItem> ChipListView
        {
            get => chipListView;
            set
            {
                chipListView = value;
                OnPropertyChanged();
            }
        }
        public ChipItems()
        {
            ChipListView = new ObservableCollection<ChipItem>();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
