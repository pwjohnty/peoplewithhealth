using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class question
    {

        //[JsonIgnore]
        public string questionid { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        //public bool deleted { get; set; }
        public string title { get; set; }
        public string directions { get; set; }
        public string type { get; set; }
        public string questionnaireid { get; set; }
        public bool active { get; set; }
        public string category { get; set; }
        public string notes { get; set; }
        public string questionorder { get; set; }
        public string signupcodereferral { get; set; }
        public string area { get; set; }
        public string externalid { get; set; }


    }

    public class ApiResponseQuestion
    {
        public ObservableCollection<question> Value { get; set; }
    }
}
