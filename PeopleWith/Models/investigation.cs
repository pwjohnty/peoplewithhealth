using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class investigation
    {
            [System.Text.Json.Serialization.JsonIgnore]
            public string investigationid { get; set; }
            public bool deleted { get; set; }
            public string investigationtitle { get; set; }
            public string investigationinformation { get; set; }
            public string active { get; set; }
            public string SNOMED { get; set; }
            public string ICD10 { get; set; }
            public string grouping { get; set; }
            public string description { get; set; }

            [System.Text.Json.Serialization.JsonIgnore]
            public string ShortGroup { get; set; }
     }

    public class APInvestigationResponse
    {
        public ObservableCollection<investigation> Value { get; set; }
    }
}
