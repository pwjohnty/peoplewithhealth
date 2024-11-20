using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class userfeedback
    {

        [System.Text.Json.Serialization.JsonIgnore]
        public string id { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        //public bool deleted { get; set; }

        public string userid { get; set; }
        public string symptomfeedback { get; set; }
        public string medicationfeedback { get; set; }
        public string supplementfeedback { get; set; }
        public string measurementfeedback { get; set; }
        public string moodfeedback { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<feedbackdata> symptomfeedbacklist { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<feedbackdata> medicationfeedbacklist { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<feedbackdata> supplementfeedbacklist { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<feedbackdata> measurementfeedbacklist { get; set; }


        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<feedbackdata> moodfeedbacklist { get; set; }


    }

    public class ApiResponseUserFeedback
    {
        public ObservableCollection<userfeedback> Value { get; set; }
    }
}
