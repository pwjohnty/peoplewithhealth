using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class answer
    {

        //[JsonIgnore]
        public string answerid { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        //public bool deleted { get; set; }
        public string answervalue { get; set; }
        public string label { get; set; }
        public string questionid { get; set; }
        public bool active { get; set; }
        public string notes { get; set; }
        public string order { get; set; }

        public string signupcodereferral { get; set; }


    }

    public class ApiResponseAnswer
    {
        public ObservableCollection<answer> Value { get; set; }
    }
}
