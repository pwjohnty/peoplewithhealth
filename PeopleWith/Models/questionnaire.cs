using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class questionnaire
    {

        //[JsonIgnore]
        public string questionnaireid { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        public bool deleted { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string questionnaireinformation { get; set; }
        public bool active { get; set; }
        public string author { get; set; }
        public string signupcodeid { get; set; }
        public string referral { get; set; }
        public string medicationid { get; set; }
        public string symptomid { get; set; }

        public string measurementid { get; set; }
        public string supplementid { get; set; }
        public string gender { get; set; }
        public string age { get; set; }

        // [JsonConverter(typeof(QuestionnaireQuestionConverter))]
        public string questionanswerjson { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [JsonConverter(typeof(QuestionnaireQuestionConverter))]
        public ObservableCollection<questionanswerinfo> questionanswerjsonlist { get; set; }

    }

    public class ApiResponseQuestionnaire
    {
        public ObservableCollection<questionnaire> Value { get; set; }
    }
}
