using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class usersupplement
    {
        //[JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public string id { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        //public bool deleted { get; set; }
        public string userid { get; set; }
        public string supplementid { get; set; }
        public string supplementtitle { get; set; }
        public string preparation { get; set; }
        public string formulation { get; set; }
        public string unit { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string frequency { get; set; }

        [JsonConverter(typeof(MedSuppTimesConverter))]
        public ObservableCollection<MedtimesDosages> schedule { get; set; }
        public string diagnosis { get; set; }
        public string status { get; set; }
        public string feedback { get; set; }
        //public string details { get; set; }

    }

    public class ApiResponseUserSupplement
    {
        public ObservableCollection<usersupplement> Value { get; set; }
    }
}
