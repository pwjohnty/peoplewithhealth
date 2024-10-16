using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class supplement
    {

        //[JsonIgnore]
        public string supplementid { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        //public bool deleted { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string supplementinformation { get; set; }
        public string status { get; set; }
        public string userid { get; set; }
        public string SNOWMED { get; set; }
        public string ICD10 { get; set; }
        public string BNFCode { get; set; }
        public string DMDProductDescription { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool AlreadySelected { get; set; }


    }

    public class ApiResponseSupplement
    {
        public ObservableCollection<supplement> Value { get; set; }
    }
}
