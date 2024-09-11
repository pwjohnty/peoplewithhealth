using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class userresponse
    {

        [System.Text.Json.Serialization.JsonIgnore]
        public string id { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        //public bool deleted { get; set; }
        public string userid { get; set; }
        public string questionid { get; set; }
        public string answerid { get; set; }
        public string userquestionnaireid { get; set; }
        public string notes { get; set; }
        public string score { get; set; }
        public string responsedate { get; set; }


    }

    public class ApiResponseUserResponse
    {
        public ObservableCollection<userresponse> Value { get; set; }
    }
}
