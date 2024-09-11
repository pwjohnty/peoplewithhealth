using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class measurement
    {

        //[JsonIgnore]
        public string measurementid { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        //public bool deleted { get; set; }
        public string measurementname { get; set; }
        public string measurementStatus { get; set; }
        public string units { get; set; }


    }

    public class ApiResponseMeasurement
    {
        public ObservableCollection<measurement> Value { get; set; }
    }
}
