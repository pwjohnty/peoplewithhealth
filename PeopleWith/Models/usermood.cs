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
    public class usermood
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string id { get; set; }
        public bool deleted { get; set; }
        public string title { get; set; }
        public string datetime { get; set; }
        public string userid { get; set; }
        public string notes { get; set; }

        //Set Image Source

        [System.Text.Json.Serialization.JsonIgnore]
        public string source { get; set; }
    }
}
