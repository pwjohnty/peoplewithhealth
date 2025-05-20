using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class userfitnessdata
    {

        [System.Text.Json.Serialization.JsonIgnore]
        public string fitnessid { get; set; }
     

        public string userid { get; set; }
        public string stepfeedback { get; set; }
        public string distancefeedback { get; set; }
        public string heartratefeedback { get; set; }
        public string respiratoryratefeedback { get; set; }

        public string additionalanswers { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<fitnessfeedback> stepfeedbacklist { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<fitnessfeedback> distancefeedbacklist { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<fitnessfeedback> heartratefeedbacklist { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<fitnessfeedback> respiratoryratefeedbacklist { get; set; }


  
    }


    public class ApiResponseUserFitness
    {
        public ObservableCollection<userfitnessdata> Value { get; set; }
    }

}
