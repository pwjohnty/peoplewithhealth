using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class userinvestigation
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string id { get; set; }
        public bool deleted { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime createdAt { get; set; }
        public string userid { get; set; }
        public string investigationid { get; set; }
        public string investigationname { get; set; }
        public string value { get; set; }
        public string status { get; set; }
        public string investigationdate { get; set; }
        public string investigationdocument { get; set; }
        public string investigationimage { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string datewotime { get; set; }

        [JsonConverter(typeof(InvestigationsConverter))]
        public ObservableCollection<notesuserfeedback> notes { get; set; }
    }

    public class rawuserinvestigation
    {
        public string id { get; set; }
        public bool deleted { get; set; }
        public DateTime createdAt { get; set; }
        public string userid { get; set; }
        public string investigationid { get; set; }
        public string investigationname { get; set; }
        public string value { get; set; }
        public string status { get; set; }
        public string investigationdate { get; set; }
        public string investigationdocument { get; set; }
        public string investigationimage { get; set; }
        public string notes { get; set; }
    }

    public class notesuserfeedback
    {
        public string id { get; set; }
        public string deleted { get; set; }
        public string datetime { get; set; }
        public string notes { get; set; }
    }

    public class APIUserInvestigationResponse
    {
        public ObservableCollection<userinvestigation> Value { get; set; }
    }

    public class SingleUserINvestigation
    {
        public ObservableCollection<rawuserinvestigation> Value { get; set; }
    }
}
