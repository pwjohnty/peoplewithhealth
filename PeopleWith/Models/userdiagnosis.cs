using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class userdiagnosis
    {

        //[JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public string id { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        //public bool deleted { get; set; }
        public string userid { get; set; }
        public string diagnosisid { get; set; }
        public string dateofdiagnosis { get; set; }
        public string diagnosistitle { get; set; }
        public string additionalparameters { get; set; }
        public string notes { get; set; }

    }

    public class ApiResponseUserDiagnosis
    {
        public ObservableCollection<userdiagnosis> Value { get; set; }
    }
}
