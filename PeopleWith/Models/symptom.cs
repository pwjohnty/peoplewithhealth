using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class symptom
    {

        //[JsonIgnore]
        public string symptomid { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        //public bool deleted { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string userid { get; set; }
        public string classification { get; set; }
        public string symptominformation { get; set; }
        public bool bodymapinput { get; set; }
        public string episodeinput { get; set; }
        public bool sliderinput { get; set; }
        public string grouping { get; set; }
        public string SNOWMED { get; set; }
        public string ICD10 { get; set; }




    }

    public class ApiResponseSymptom
    {
        public ObservableCollection<symptom> Value { get; set; }
    }
}
