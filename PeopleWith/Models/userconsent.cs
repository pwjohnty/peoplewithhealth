using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class userconsent
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string id { get; set; }
        public bool deleted { get; set; }
        public string userid { get; set; }
        public string consentid { get; set; }
        public string consentselection { get; set; }
        public string signaturefilename { get; set; }
        public string consentinput { get; set; }
    }
    public class ApiResponeUserConsent
    {
        public ObservableCollection<userconsent> Value { get; set; }
    }
}
