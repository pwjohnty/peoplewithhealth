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
    public class crashlog
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string id { get; set; }
        public bool deleted { get; set; }
        public string page { get; set; }
        public string line { get; set; }
        public string exception { get; set; }
        public string stacktrace { get; set; }
        public string userid { get; set; }
        public string deviceos { get; set; }
        public string devicemodel { get; set; }
        public bool acknowledged { get; set; }
        public bool actioned { get; set; }
        public bool resolved { get; set; }

    }
}
