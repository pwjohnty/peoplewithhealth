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
    public class registryData
    {
        //[JsonIgnore]
        public string id { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime createdAt { get; set; }
        public bool deleted { get; set; }
        public string userid { get; set; }
        public string webuserid { get; set; }
        public string inputid { get; set; }
        public string inputValue { get; set; }
        public string condition { get; set; }
        public string advertid { get; set; }
        public string notes { get; set; }
        public string inputConditionCategory { get; set; }
        public bool userPermitted { get; set; }

        public string dateTime { get; set; }

        public string firstDataPoint { get; set; }

        public bool currentDataPoint { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string Title { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string Question { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool HasNotes { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string DateConverted { get; set; }


    }

    public class ApiResponseregistyData
    {
        public ObservableCollection<registryData> Value { get; set; }
    }
}
