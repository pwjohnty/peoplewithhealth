using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class userquestionnaire
    {

        [System.Text.Json.Serialization.JsonIgnore]
        public string userquestionnaireid { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        public bool deleted { get; set; }
        public string userid { get; set; }
        public string questionnaireid { get; set; }
        public string completedatetime { get; set; }

        public string questionanswerjson { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<userquestionnaireresponse> questionanswerjsonlist { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]

        public string questionnairename { get; set; }
    }

    public class ApiResponseUserQuestionnaire
    {
        public ObservableCollection<userquestionnaire> Value { get; set; }
    }
}
