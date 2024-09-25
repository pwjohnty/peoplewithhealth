using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class MedSuppFeedback : INotifyPropertyChanged
    {

        //[JsonIgnore]

        public string id { get; set; }
        public string Recorded { get; set; }

        public string datetime { get; set; }
    

    

        // event handler for updating the list views
        public event PropertyChangedEventHandler PropertyChanged;
        //  public void OnPropertyChanged()
        //{
        //  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs());
        // PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        // }
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }


}
