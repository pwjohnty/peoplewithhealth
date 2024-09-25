using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class preparation
    {

        //[JsonIgnore]
        public string preparationid { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        //public bool deleted { get; set; }
        public string title { get; set; }
        public string formulation { get; set; }
        public string unit { get; set; }

    }

    public class ApiResponsePrepartion
    {
        public ObservableCollection<preparation> Value { get; set; }
    }
}
