using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class appversion
    {

        //[System.Text.Json.Serialization.JsonIgnore]
        public string appversionid { get; set; }
        //[System.Text.Json.Serialization.JsonIgnore]
        public string iosversion { get; set; }
        //[System.Text.Json.Serialization.JsonIgnore]
        public string androidversion { get; set; }
  
    }

    public class ApiAppVersion
    {
        public ObservableCollection<appversion> Value { get; set; }
    }
}
