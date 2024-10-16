using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace PeopleWith
{
    public class hcp
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string hcpid { get; set; }
        public bool deleted { get; set; }
        public string userid { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string fullname { get; set; }
        public string firstname { get; set; }
        public string surname { get; set; }
        public string locationname { get; set; }
        public string role { get; set; }
        public string addresslineone { get; set; }
        public string addresslinetwo { get; set; }
        public string addresslinethree { get; set; }
        public string towncity { get; set; }
        public string county { get; set; }
        public string country { get; set; }
        public string postcode { get; set; }
        public string telephone { get; set; }
        public string email { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string ContactTitle { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string ContactItem { get; set; }

    }
    public class ApiResponeHCP
    {
        public ObservableCollection<hcp> Value { get; set; }
    }
}
