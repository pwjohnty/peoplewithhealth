using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class medication
    {

        //[JsonIgnore]
        public string medicationid { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        //public bool deleted { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string medicationinformation { get; set; }
        public string status { get; set; }
        public string userid { get; set; }
        public string SNOWMED { get; set; }
        public string ICD10 { get; set; }
        public string BNFCode { get; set; }
        public string DMDProductDescription { get; set; }

    }

    public class ApiResponseMedication
    {
        public ObservableCollection<medication> Value { get; set; }
    }
}
