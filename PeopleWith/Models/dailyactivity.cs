using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class dailyactivity
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string activityid { get; set; }
        public bool deleted { get; set; }
        public string activitytitle { get; set; }
        public string activitydescription { get; set; }
        public string activityinformation { get; set; }
        public string activityoptions { get; set; }
        public string activityfrequency { get; set; }
        public string grouping { get; set; }
        public string notes { get; set; }
        public bool active { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string ShortGroup { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string Source { get; set; }
    }

    public class APIActivityResponse
    {
        public ObservableCollection<dailyactivity> Value { get; set; }
    }
}
