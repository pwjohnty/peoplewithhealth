using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class postcode
    {

        //[JsonIgnore]
        public string id { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        //public bool deleted { get; set; }
        public string postcodebrick { get; set; }
        public string county { get; set; }
        public string region { get; set; }
  


    }

    public class ApiResponsePostcode
    {
        public ObservableCollection<postcode> Value { get; set; }
    }
}
