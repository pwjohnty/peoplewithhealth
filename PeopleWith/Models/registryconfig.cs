using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class registryconfig
    {

        [System.Text.Json.Serialization.JsonIgnore]
        public string id { get; set; }
        public bool deleted { get; set; }
        public string advertID { get; set; }
        public string registryCondition { get; set; }

    }

    public class ApiRegConfig
    {
        public ObservableCollection<registryconfig> Value { get; set; }
    }
}
