using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class consent
    {

        [System.Text.Json.Serialization.JsonIgnore]
        public string consentid { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        //public bool deleted { get; set; }
        public string signupcodeid { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public string content { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string area { get; set; }

        public bool additionalconsent { get; set; }

        public bool signaturepad { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string exitid { get; set; }
    }

    public class ApiResponseConsent
    {
        public ObservableCollection<consent> Value { get; set; }
    }
}
