using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class diet
    {

           [System.Text.Json.Serialization.JsonIgnore]
            public string dietid { get; set; }
            public bool deleted { get; set; }
            public string diettitle { get; set; }
            public string dietinformation { get; set; }
            public string dietdescription { get; set; }
            public string SNOMED { get; set; }
            public string grouping { get; set; }

            [System.Text.Json.Serialization.JsonIgnore]
            public string ShortGroup { get; set; }
    }

    public class APIDietResponse
    {
        public ObservableCollection<diet> Value { get; set; }
    }
}
