using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class feedbackdata
    {
        //Made nullable to handle null items steralise
        public string id { get; set; }

        public string datetime { get; set; }
        public string action { get; set; }
        public string value { get; set; }
        public string label { get; set; }
        public string unit { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string labelandunit { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string medicationfeedbackid { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public List<feedbackdata> symptomlist { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string symptominfo { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ImageSource img { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string improvelbl { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string datetimestring { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public int Count { get; set; }


        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime dt { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool showchart { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string avgstring { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string highstring { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string nooftimesstring { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string title { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string shortlabel { get; set; }

        //[System.Text.Json.Serialization.JsonIgnore]
        //public string shortunit { get; set; }

    }

}
