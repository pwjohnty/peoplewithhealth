using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PeopleWith
{
    public class privacypolicy
    {
        public string policyid { get; set; }
        public bool deleted { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string signupcodeid { get; set; }

    }

    public class ApiPrivPolicy
    {
        public ObservableCollection<privacypolicy> Value { get; set; }
    }
}
