using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class videoengage
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string id { get; set; }
        public bool deleted { get; set; }
        public string userid { get; set; }
        public string videoid { get; set; }
        public string datetimeaccessed { get; set; }
        public string watchduration { get; set; }
        public string pauseduration { get; set; }
        public string closeaction { get; set; }

    }

    public class ApiResponseVideoEngage
    {
        public ObservableCollection<videoengage> Value { get; set; }
    }
}
