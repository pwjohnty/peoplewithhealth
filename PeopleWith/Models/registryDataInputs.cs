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
    public class registryDataInputs 
    {

        //[JsonIgnore]
        public string id { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        //public bool deleted { get; set; }
        public string dataInputs { get; set; }
        public string dataTab { get; set; }
        public string label { get; set; }
        public string labelDirections { get; set; }
        public string type { get; set; }
        public string values { get; set; }
        public string upperLimit { get; set; }
        public string lowerLimit { get; set; }
        public bool active { get; set; }

        public string access { get; set; }

        public string subType { get; set; }

        public bool chart { get; set; }

        public bool patientAccess { get; set; }

        public string itemOrder { get; set; }

        public string inputGroup { get; set; }

        public string trigger { get; set; }

        public string webOnly { get; set; }

        public string apporder { get; set; }

    }

    public class ApiResponseregistyDataInputs
    {
        public ObservableCollection<registryDataInputs> Value { get; set; }
    }
}
