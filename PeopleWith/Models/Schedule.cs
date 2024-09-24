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
    public class Schedule : INotifyPropertyChanged
    {

        //[JsonIgnore]


        private string day;
        public string Day
        {
            get { return day; }
            set
            {
                day = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Day)));
                }
            }
        }

        public string dosageunit { get; set; }

        private string date;
        public string Date
        {
            get { return date; }
            set
            {
                date = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Date)));
                }
            }
        }

        private Color bgcolour;
        public Color Bgcolour
        {
            get { return bgcolour; }
            set
            {
                bgcolour = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Bgcolour)));
                }
            }
        }


        private Color bordercolour;
        public Color Bordercolour
        {
            get { return bordercolour; }
            set
            {
                bordercolour = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Bordercolour)));
                }
            }
        }

        private double op;
        public double Op
        {
            get { return op; }
            set
            {
                op = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Op)));
                }
            }
        }

       



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
